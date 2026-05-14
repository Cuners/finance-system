using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Budgets.DeleteBudget
{
    public class DeleteBudgetUseCase : IDeleteBudgetUseCase
    {
        private readonly IBudgetRepository _budgets;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteBudgetUseCase> _logger;
        private readonly IBudgetCacheInvalidator _cache;

        public DeleteBudgetUseCase(
            IBudgetRepository budgets,
            IUnitOfWork unitOfWork,
            ILogger<DeleteBudgetUseCase> logger,
            IBudgetCacheInvalidator cache)
        {
            _budgets = budgets;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<DeleteBudgetResult>> ExecuteAsync(
            DeleteBudgetCommand command,
            int userId,
            CancellationToken ct)
        {
            try
            {
                var budget = await _budgets.GetBudgetById(command.BudgetId, ct);
                if (budget == null || budget.UserId != userId)
                {
                    return Result<DeleteBudgetResult>.Failure(
                        "BUDGET_ACCESS_DENIED",
                        "Budget not found or access denied");
                }

                await _budgets.DeleteBudget(command.BudgetId);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, command.BudgetId, ct);

                return Result<DeleteBudgetResult>.Success(new DeleteBudgetResult(command.BudgetId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete budget {BudgetId} for user {UserId}", command.BudgetId, userId);
                return Result<DeleteBudgetResult>.Failure(
                    "BUDGET_DELETE_FAILED",
                    "Unable to delete budget at this time");
            }
        }
    }
}
