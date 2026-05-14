using Finance.Application.Common;
using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Budgets.GetBudgetsByUserId
{
    public class GetBudgetsByUserIdUseCase : IGetBudgetsByUserIdUseCase
    {
        private readonly IBudgetRepository _budgets;
        private readonly ILogger<GetBudgetsByUserIdUseCase> _logger;
        private readonly ICacheService _cache;

        public GetBudgetsByUserIdUseCase(
            IBudgetRepository budgets,
            ILogger<GetBudgetsByUserIdUseCase> logger,
            ICacheService cache)
        {
            _budgets = budgets;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<GetBudgetsByUserIdResult>> ExecuteAsync(
            GetBudgetsByUserIdQuery query,
            int userId,
            CancellationToken ct)
        {
            var cacheKey = $"budgets:user:{userId}:byId";

            try
            {
                var budgets = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    return await BuildBudgetDtosAsync(userId, token);
                });

                return Result<GetBudgetsByUserIdResult>.Success(
                    new GetBudgetsByUserIdResult(budgets ?? []));
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, "Cache timeout while getting budgets for user {UserId}", userId);
                var budgets = await BuildBudgetDtosAsync(userId, ct);
                return Result<GetBudgetsByUserIdResult>.Success(new GetBudgetsByUserIdResult(budgets));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get budgets for user {UserId}", userId);
                return Result<GetBudgetsByUserIdResult>.Failure(
                    "BUDGETS_GET_FAILED",
                    "Unable to get budgets at this time");
            }
        }

        private async Task<IReadOnlyList<BudgetDto>> BuildBudgetDtosAsync(
            int userId,
            CancellationToken ct)
        {
            var budgets = await _budgets.GetBudgetsByUserId(userId, ct);

            return budgets.Select(x => new BudgetDto
            {
                BudgetId = x.BudgetId,
                Name = x.Name,
                LimitAmount = x.LimitAmount,
                Date = x.Date,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name
            }).ToList();
        }
    }
}
