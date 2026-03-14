using Finance.Application.Services;
using Finance.Application.UseCases.Budgets.UpdateBudget.Request;
using Finance.Application.UseCases.Budgets.UpdateBudget.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.UpdateBudget
{
    public class UpdateBudgetUseCase : IUseCase<UpdateBudgetRequest, UpdateBudgetResponse>
    {
        private readonly IBudgetRepository _Budget;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateBudgetUseCase> _logger;
        private readonly IBudgetCacheInvalidator _cache;
        public UpdateBudgetUseCase(IBudgetRepository Budget, 
                                   IUnitOfWork unitOfWork, 
                                   ILogger<UpdateBudgetUseCase> logger,
                                   IBudgetCacheInvalidator cache)
        {
            _Budget = Budget;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }
        public async Task<UpdateBudgetResponse> ExecuteAsync(UpdateBudgetRequest request, int userId, CancellationToken ct)
        {
            try
            {
                var Budget = await _Budget.GetBudgetById(request.BudgetId,ct);
                Budget.Name = request.Name;
                Budget.CategoryId = request.CategoryId;
                await _Budget.UpdateBudget(Budget);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, request.BudgetId, ct);
                return new UpdateBudgetSuccessResponse(Budget.BudgetId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message,request.BudgetId);
                return new UpdateBudgetErrorResponse("Unable to update Budget at this time", "INVALID_UPDATE");
            }
        }
    }
}
