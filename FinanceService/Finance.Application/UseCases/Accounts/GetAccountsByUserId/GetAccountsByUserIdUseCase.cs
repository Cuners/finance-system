using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId.Request;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId.Response;
using Finance.Application.UseCases.Accounts.GetValueAccounts.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetAccountsByUserId
{
    public class GetAccountsByUserIdUseCase : IUseCase<GetAccountsByUserIdRequest, GetAccountsByUserIdResponse>
    {
        private readonly IAccountRepository _accounts;
        private readonly ITransactionRepository _transaction;
        private readonly ILogger<GetAccountsByUserIdUseCase> _logger;
        private readonly ICacheService _cache;
        public GetAccountsByUserIdUseCase(IAccountRepository accounts, 
                                          ILogger<GetAccountsByUserIdUseCase> logger,
                                          ITransactionRepository transaction,
                                          ICacheService cache)
        {
            _accounts = accounts;
            _logger=logger;
            _transaction=transaction;
            _cache=cache;
        }

        public async Task<GetAccountsByUserIdResponse> ExecuteAsync(GetAccountsByUserIdRequest request, int userId, CancellationToken ct)
        {
            var cacheKey = $"accounts:user:{userId}:";
            try
            {
               
                var accountValue = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    var accounts = await _accounts.GetAccountsByUserId(userId, ct);
                    var sumTransactions = await _transaction.GetSummaryTransactionsByAccounts(userId, ct);

                    var result = accounts.Select(a =>
                    {
                        var stats = sumTransactions.FirstOrDefault(t => t.AccountId == a.AccountId);
                        return new AccountSummaryDto
                        {
                            AccountId = a.AccountId,
                            Name = a.Name,
                            Balance = a.Balance,
                            Note = a.Note,
                            Income = stats?.Income ?? 0,
                            Expense = stats?.Expense ?? 0,
                            Count = stats?.Count ?? 0
                        };
                    }).ToList();
                    return result;
                });
                if(accountValue is null)
                {
                    return new GetAccountsByUserIdErrorResponse("No accounts found", "ACCOUNTS_NOT_FOUND");
                }
                return new GetAccountsByUserIdSuccessResponse(accountValue);
            }
            catch (TimeoutException ex)
            {
                var accounts = await _accounts.GetAccountsByUserId(userId, ct);
                var sumTransactions = await _transaction.GetSummaryTransactionsByAccounts(userId, ct);
                if (accounts == null || !accounts.Any())
                {
                    _logger.LogWarning("GetAccountsByUserIdRequest account is null");
                    return new GetAccountsByUserIdErrorResponse("No accounts found", "ACCOUNTS_NOT_FOUND");
                }
                var result = accounts.Select(a =>
                {
                    var stats = sumTransactions.FirstOrDefault(t => t.AccountId == a.AccountId);
                    return new AccountSummaryDto
                    {
                        AccountId = a.AccountId,
                        Name = a.Name,
                        Balance = a.Balance,
                        Note = a.Note,
                        Income = stats?.Income ?? 0,
                        Expense = stats?.Expense ?? 0,
                        Count = stats?.Count ?? 0
                    };
                }).ToList();
                return new GetAccountsByUserIdSuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetAccountsByUserIdErrorResponse("No accounts found", "ACCOUNTS_NOT_FOUND");
            }
        }
    }
}
