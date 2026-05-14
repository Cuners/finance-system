using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Budgets.GetBudgetsStatus
{
    public class GetBudgetsStatusUseCase : IGetBudgetsStatusUseCase
    {
        private readonly IBudgetRepository _budgets;
        private readonly ILogger<GetBudgetsStatusUseCase> _logger;
        private readonly ICacheService _cache;

        public GetBudgetsStatusUseCase(
            IBudgetRepository budgets,
            ILogger<GetBudgetsStatusUseCase> logger,
            ICacheService cache)
        {
            _budgets = budgets;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<GetBudgetsStatusResult>> ExecuteAsync(
            GetBudgetsStatusQuery query,
            int userId,
            CancellationToken ct)
        {
            if (userId <= 0)
            {
                return Result<GetBudgetsStatusResult>.Failure("INVALID_USER_ID", "Invalid user id");
            }

            var cacheKey = $"dashboard:user:{userId}:budgetsStatus";

            try
            {
                var statuses = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    return await BuildBudgetStatusesAsync(userId, token);
                });

                return Result<GetBudgetsStatusResult>.Success(
                    new GetBudgetsStatusResult(statuses ?? []));
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, "Cache timeout while getting budget statuses for user {UserId}", userId);
                var statuses = await BuildBudgetStatusesAsync(userId, ct);
                return Result<GetBudgetsStatusResult>.Success(new GetBudgetsStatusResult(statuses));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get budget statuses for user {UserId}", userId);
                return Result<GetBudgetsStatusResult>.Failure(
                    "BUDGET_STATUSES_GET_FAILED",
                    "Unable to get budget statuses at this time");
            }
        }

        private async Task<IReadOnlyList<BudgetStatus>> BuildBudgetStatusesAsync(
            int userId,
            CancellationToken ct)
        {
            var statuses = await _budgets.GetBudgetStatusAsync(userId, ct);
            return statuses.ToList();
        }
    }
}
