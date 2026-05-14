using Finance.Application.Common;
using Finance.Application.DTO;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Budgets.GetBudgetById
{
    public class GetBudgetByIdUseCase : IGetBudgetByIdUseCase
    {
        private readonly IBudgetRepository _budgets;
        private readonly ILogger<GetBudgetByIdUseCase> _logger;

        public GetBudgetByIdUseCase(
            IBudgetRepository budgets,
            ILogger<GetBudgetByIdUseCase> logger)
        {
            _budgets = budgets;
            _logger = logger;
        }

        public async Task<Result<GetBudgetByIdResult>> ExecuteAsync(
            GetBudgetByIdQuery query,
            int userId,
            CancellationToken ct)
        {
            if (query.BudgetId <= 0)
            {
                return Result<GetBudgetByIdResult>.Failure("INVALID_BUDGET_ID", "Invalid budget id");
            }

            try
            {
                var budget = await _budgets.GetBudgetById(query.BudgetId, ct);
                if (budget == null || budget.UserId != userId)
                {
                    return Result<GetBudgetByIdResult>.Failure(
                        "BUDGET_ACCESS_DENIED",
                        "No budget found or access denied");
                }

                var dto = new BudgetDto
                {
                    BudgetId = budget.BudgetId,
                    Name = budget.Name,
                    LimitAmount = budget.LimitAmount,
                    Date = budget.Date,
                    CategoryId = budget.CategoryId,
                    CategoryName = budget.Category.Name
                };

                return Result<GetBudgetByIdResult>.Success(new GetBudgetByIdResult(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get budget {BudgetId} for user {UserId}", query.BudgetId, userId);
                return Result<GetBudgetByIdResult>.Failure(
                    "BUDGET_GET_FAILED",
                    "Unable to get budget at this time");
            }
        }
    }
}
