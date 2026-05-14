using Finance.Application.Common;
using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Budgets.GetBudgetsSummary
{
    public class GetBudgetsSummaryUseCase : IGetBudgetsSummaryUseCase
    {
        private readonly IBudgetRepository _budgets;
        private readonly ILogger<GetBudgetsSummaryUseCase> _logger;
        private readonly ICacheService _cache;

        public GetBudgetsSummaryUseCase(
            IBudgetRepository budgets,
            ILogger<GetBudgetsSummaryUseCase> logger,
            ICacheService cache)
        {
            _budgets = budgets;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<GetBudgetsSummaryResult>> ExecuteAsync(
            GetBudgetsSummaryQuery query,
            int userId,
            CancellationToken ct)
        {
            var cacheKey = $"dashboard:user:{userId}:budgetsSummary";

            try
            {
                var summary = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    return await BuildBudgetSummaryAsync(userId, token);
                });

                return Result<GetBudgetsSummaryResult>.Success(
                    new GetBudgetsSummaryResult(summary ?? new BudgetSummaryDto(0, 0, 0, 0)));
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, "Cache timeout while getting budget summary for user {UserId}", userId);
                var summary = await BuildBudgetSummaryAsync(userId, ct);
                return Result<GetBudgetsSummaryResult>.Success(new GetBudgetsSummaryResult(summary));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get budget summary for user {UserId}", userId);
                return Result<GetBudgetsSummaryResult>.Failure(
                    "BUDGET_SUMMARY_GET_FAILED",
                    "Unable to get budget summary at this time");
            }
        }

        private async Task<BudgetSummaryDto> BuildBudgetSummaryAsync(
            int userId,
            CancellationToken ct)
        {
            var budgets = await _budgets.GetBudgetStatusAsync(userId, ct);
            var totalBudget = budgets.Sum(b => b.LimitAmount);
            var totalSpent = budgets.Sum(b => b.TotalSpent);
            var remaining = totalBudget - totalSpent;
            var procentSpent = totalBudget == 0 ? 0 : Math.Round(totalSpent / (totalBudget / 100));

            return new BudgetSummaryDto(
                TotalBudget: totalBudget,
                TotalSpent: totalSpent,
                ProcentSpent: procentSpent,
                Remaining: remaining);
        }
    }
}
