using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Budgets.UpdateBudget
{
    public class UpdateBudgetUseCase : IUpdateBudgetUseCase
    {
        private readonly IBudgetRepository _budgets;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateBudgetUseCase> _logger;
        private readonly IBudgetCacheInvalidator _cache;

        public UpdateBudgetUseCase(
            IBudgetRepository budgets,
            IUnitOfWork unitOfWork,
            ILogger<UpdateBudgetUseCase> logger,
            IBudgetCacheInvalidator cache)
        {
            _budgets = budgets;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<UpdateBudgetResult>> ExecuteAsync(
            UpdateBudgetCommand command,
            int userId,
            CancellationToken ct)
        {
            try
            {
                var budget = await _budgets.GetBudgetById(command.BudgetId, ct);
                if (budget == null || budget.UserId != userId)
                {
                    return Result<UpdateBudgetResult>.Failure(
                        "BUDGET_ACCESS_DENIED",
                        "Budget not found or access denied");
                }

                budget.Name = command.Name;
                budget.LimitAmount = command.LimitAmount;
                budget.CategoryId = command.CategoryId;

                await _budgets.UpdateBudget(budget);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, command.BudgetId, ct);

                return Result<UpdateBudgetResult>.Success(new UpdateBudgetResult(budget.BudgetId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update budget {BudgetId} for user {UserId}", command.BudgetId, userId);
                return Result<UpdateBudgetResult>.Failure(
                    "BUDGET_UPDATE_FAILED",
                    "Unable to update budget at this time");
            }
        }
    }
}
