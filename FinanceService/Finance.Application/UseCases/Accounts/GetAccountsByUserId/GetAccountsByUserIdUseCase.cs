using Finance.Application.Common;
using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Accounts.GetAccountsByUserId
{
    public class GetAccountsByUserIdUseCase : IGetAccountsByUserIdUseCase
    {
        private readonly IAccountRepository _accounts;
        private readonly ITransactionRepository _transaction;
        private readonly ILogger<GetAccountsByUserIdUseCase> _logger;
        private readonly ICacheService _cache;

        public GetAccountsByUserIdUseCase(
            IAccountRepository accounts,
            ILogger<GetAccountsByUserIdUseCase> logger,
            ITransactionRepository transaction,
            ICacheService cache)
        {
            _accounts = accounts;
            _logger = logger;
            _transaction = transaction;
            _cache = cache;
        }

        public async Task<Result<GetAccountsByUserIdResult>> ExecuteAsync(
            GetAccountsByUserIdQuery query,
            int userId,
            CancellationToken ct)
        {
            var cacheKey = $"accounts:user:{userId}:";

            try
            {
                var accountValue = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    return await BuildAccountSummariesAsync(userId, token);
                });

                return Result<GetAccountsByUserIdResult>.Success(
                    new GetAccountsByUserIdResult(accountValue ?? []));
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, "Cache timeout while getting accounts for user {UserId}", userId);
                var result = await BuildAccountSummariesAsync(userId, ct);
                return Result<GetAccountsByUserIdResult>.Success(
                    new GetAccountsByUserIdResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get accounts for user {UserId}", userId);
                return Result<GetAccountsByUserIdResult>.Failure(
                    "ACCOUNTS_GET_FAILED",
                    "Unable to get accounts at this time");
            }
        }

        private async Task<IReadOnlyList<AccountSummaryDto>> BuildAccountSummariesAsync(
            int userId,
            CancellationToken ct)
        {
            var accounts = await _accounts.GetAccountsByUserId(userId, ct);
            var sumTransactions = await _transaction.GetSummaryTransactionsByAccounts(userId, ct);

            return accounts.Select(a =>
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
        }
    }
}
