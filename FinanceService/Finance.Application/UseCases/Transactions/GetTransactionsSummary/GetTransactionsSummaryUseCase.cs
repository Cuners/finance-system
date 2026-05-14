using Finance.Application.Common;
using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Transactions.GetTransactionsSummary
{
    public class GetTransactionsSummaryUseCase : IGetTransactionsSummaryUseCase
    {
        private readonly ITransactionRepository _transactions;
        private readonly ILogger<GetTransactionsSummaryUseCase> _logger;
        private readonly ICacheService _cache;

        public GetTransactionsSummaryUseCase(
            ITransactionRepository transactions,
            ILogger<GetTransactionsSummaryUseCase> logger,
            ICacheService cache)
        {
            _transactions = transactions;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<GetTransactionsSummaryResult>> ExecuteAsync(
            GetTransactionsSummaryQuery query,
            int userId,
            CancellationToken ct)
        {
            if (userId <= 0)
            {
                return Result<GetTransactionsSummaryResult>.Failure("INVALID_USER_ID", "Invalid user id");
            }

            var cacheKey = $"dashboard:user:{userId}:transactionsSummary:from:{query.Year}-{query.Month:D2}";

            try
            {
                var summary = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    return await BuildSummaryAsync(userId, query.Year, query.Month, token);
                });

                return Result<GetTransactionsSummaryResult>.Success(
                    new GetTransactionsSummaryResult(summary ?? new TransactionSummaryDto(0, 0, 0)));
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, "Cache timeout while getting transactions summary for user {UserId}", userId);
                var summary = await BuildSummaryAsync(userId, query.Year, query.Month, ct);
                return Result<GetTransactionsSummaryResult>.Success(new GetTransactionsSummaryResult(summary));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get transactions summary for user {UserId}", userId);
                return Result<GetTransactionsSummaryResult>.Failure(
                    "TRANSACTION_SUMMARY_GET_FAILED",
                    "Unable to get transactions summary at this time");
            }
        }

        private async Task<TransactionSummaryDto> BuildSummaryAsync(
            int userId,
            int year,
            int month,
            CancellationToken ct)
        {
            var summary = await _transactions.GetTransactionsSummaryAsync(userId, year, month, ct);
            return new TransactionSummaryDto(
                TotalIncome: summary.TotalIncome,
                TotalExpenses: summary.TotalExpenses,
                NetBalance: summary.NetChange);
        }
    }
}
