
using Finance.Application.UseCases.Budgets.GetBudgetById.Request;
using Finance.Application.UseCases.Budgets.GetBudgetById.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetById
{
    public class GetBudgetByIdUseCase : IUseCase<GetBudgetByIdRequest, GetBudgetByIdResponse>
    {
        private readonly IBudgetRepository _BudgetRepository;
        private readonly ILogger<GetBudgetByIdUseCase> _logger;
        public GetBudgetByIdUseCase(IBudgetRepository BudgetRepository, ILogger<GetBudgetByIdUseCase> logger)
        {
            _BudgetRepository = BudgetRepository;
            _logger = logger;
        }
        public async Task<GetBudgetByIdResponse> ExecuteAsync(GetBudgetByIdRequest request, CancellationToken ct)
        {
            if (request.BudgetId <= 0)
            {
                _logger.LogWarning("GetBudgetRequest is null");
                return new GetBudgetByIdErrorResponse("Invalid Budget id", "INVALID_USER_ID");
            }
            try
            {
                var budgets = await _BudgetRepository.GetBudgetById(request.BudgetId, ct);

                if (budgets is null)
                {
                    _logger.LogWarning("GetBudgetRequest is null");
                    return new GetBudgetByIdErrorResponse("No Budget found", "Budget_NOT_FOUND");
                }
                var result = new BudgetDto
                {
                    BudgetId = budgets.BudgetId,
                    Name = budgets.Name,
                    LimitAmount = budgets.LimitAmount,
                    Date = budgets.Date,
                    CategoryId = budgets.CategoryId,
                    CategoryName=budgets.Category.Name
                };
                return new GetBudgetByIdSuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetBudgetByIdErrorResponse("Unable to get Budget at this time", "INVALID_GET");
            }
        }
    }    
}

