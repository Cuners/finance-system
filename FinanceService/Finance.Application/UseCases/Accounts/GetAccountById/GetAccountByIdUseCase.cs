
using Finance.Application.DTO;
using Finance.Application.UseCases.Accounts.GetAccountById.Request;
using Finance.Application.UseCases.Accounts.GetAccountById.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetAccountById
{
    public class GetAccountByIdUseCase : IUseCase<GetAccountByIdRequest, GetAccountByIdResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transaction;
        private readonly ILogger<GetAccountByIdUseCase> _logger;
        public GetAccountByIdUseCase(IAccountRepository accountRepository, 
                                     ILogger<GetAccountByIdUseCase> logger,
                                     ITransactionRepository transaction)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _transaction = transaction;
        }
        public async Task<GetAccountByIdResponse> ExecuteAsync(GetAccountByIdRequest request, int userId, CancellationToken ct)
        {
            try
            {
                if (request.AccountId <= 0)
                {
                    _logger.LogWarning("GetAccountRequest is invalid");
                    return new GetAccountByIdErrorResponse("Invalid account id", "INVALID_USER_ID");
                }
                var accounts = await _accountRepository.GetAccountByAccountId(request.AccountId,ct);
                if (accounts == null || accounts.UserId != userId)
                {
                    _logger.LogWarning("GetAccountRequest account is null or access denied");
                    return new GetAccountByIdErrorResponse("No account found or access denied", "ACCOUNT_ACCESS_DENIED");
                }
                var result = new AccountDto
                {
                    AccountId = accounts.AccountId,
                    Name = accounts.Name,
                    Balance = accounts.Balance,
                    Note=accounts.Note
                };
                return new GetAccountByIdSuccessResponse(result);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetAccountByIdErrorResponse("Unable to get account at this time", "INVALID_GET");
            }
        }
    }
}
