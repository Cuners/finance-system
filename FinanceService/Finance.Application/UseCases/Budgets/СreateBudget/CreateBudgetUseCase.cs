
using Finance.Application.Services;
using Finance.Application.UseCases.Budgets.СreateBudget.Request;
using Finance.Application.UseCases.Budgets.СreateBudget.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.СreateBudget
{
    public class CreateBudgetUseCase : IUseCase<CreateBudgetRequest, CreateBudgetResponse>
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBudgetUseCase> _logger;
        private readonly IBudgetCacheInvalidator _cache;
        public CreateBudgetUseCase(IBudgetRepository budgetRepository, 
                                   ICategoryRepository categoryRepository, 
                                   IUnitOfWork unitOfWork, 
                                   ILogger<CreateBudgetUseCase> logger,
                                   IBudgetCacheInvalidator cache)
        {
            _budgetRepository = budgetRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork=unitOfWork;
            _logger = logger;
            _cache = cache; 
        }
        public async Task<CreateBudgetResponse> ExecuteAsync(CreateBudgetRequest request, int userId, CancellationToken ct)
        {
            try
            {
                var budget = new Domain.Budget
                {
                    UserId = userId,
                    Name = request.Name,
                    LimitAmount = request.LimitAmount,
                    Date= DateOnly.FromDateTime(DateTime.UtcNow),
                    CategoryId = request.CategoryId
                };
                await _budgetRepository.CreateBudget(budget);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId,budget.BudgetId,ct);
                return new CreateBudgetSuccessResponse(budget.BudgetId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,ex.Message,request.Name);
                return new CreateBudgetErrorResponse("Unable TO CREATE BUDGET at this time", "INVALID_CREATE");
            }
        }
    }
}
