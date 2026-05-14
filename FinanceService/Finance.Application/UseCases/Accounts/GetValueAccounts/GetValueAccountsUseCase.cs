using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Accounts.GetValueAccounts
{
    public class GetValueAccountsUseCase : IGetValueAccountsUseCase
    {
        private readonly IAccountRepository _accounts;
        private readonly ILogger<GetValueAccountsUseCase> _logger;
        private readonly ICacheService _cache;

        public GetValueAccountsUseCase(
            IAccountRepository accounts,
            ILogger<GetValueAccountsUseCase> logger,
            ICacheService cache)
        {
            _accounts = accounts;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<GetValueAccountsResult>> ExecuteAsync(
            GetValueAccountsQuery query,
            int userId,
            CancellationToken ct)
        {
            var cacheKey = $"accounts:user:{userId}:accountValue";

            try
            {
                var accountValue = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    return await _accounts.GetTotalValue(userId, token);
                });

                return Result<GetValueAccountsResult>.Success(new GetValueAccountsResult(accountValue));
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, "Cache timeout while getting account value for user {UserId}", userId);
                var accountValue = await _accounts.GetTotalValue(userId, ct);
                return Result<GetValueAccountsResult>.Success(new GetValueAccountsResult(accountValue));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get account value for user {UserId}", userId);
                return Result<GetValueAccountsResult>.Failure(
                    "ACCOUNT_VALUE_GET_FAILED",
                    "Unable to get balance for accounts at this time");
            }
        }
    }
}
