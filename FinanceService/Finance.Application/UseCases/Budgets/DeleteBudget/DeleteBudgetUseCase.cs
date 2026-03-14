using Finance.Application.Services;
using Finance.Application.UseCases.Budgets.DeleteBudget.Request;
using Finance.Application.UseCases.Budgets.DeleteBudget.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.DeleteBudget
{
    public class DeleteBudgetUseCase : IUseCase<DeleteBudgetRequest, DeleteBudgetResponse>
    {
        private readonly IBudgetRepository _Budget;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteBudgetUseCase> _logger;
        private readonly IBudgetCacheInvalidator _cache;
        public DeleteBudgetUseCase(IBudgetRepository Budget, 
                                   IUnitOfWork unitOfWork, 
                                   ILogger<DeleteBudgetUseCase> logger,
                                   IBudgetCacheInvalidator cache)
        {
            _Budget = Budget;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }
        public async Task<DeleteBudgetResponse> ExecuteAsync(DeleteBudgetRequest request, int userId, CancellationToken ct)
        {
            try
            {
                var budget = await _Budget.GetBudgetById(request.BudgetId, ct);
                if (budget == null || budget.UserId != userId)
                {
                    return new DeleteBudgetErrorResponse("Budget not found or access denied", "BUDGET_ACCESS_DENIED");
                }
                
                await _Budget.DeleteBudget(request.BudgetId);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, request.BudgetId, ct);
                return new DeleteBudgetSuccessResponse(request.BudgetId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new DeleteBudgetErrorResponse("Unable to delete Budget at this time", "INVALID_DELETE");
            }
        }
    }
}
