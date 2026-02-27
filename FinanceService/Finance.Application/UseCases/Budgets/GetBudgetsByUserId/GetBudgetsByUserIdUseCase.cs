using Finance.Application.UseCases.Accounts;
using Finance.Application.UseCases.Budgets.GetBudgetById;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId.Request;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId.Response;
using Finance.Application.UseCases.Budgets.UpdateBudget;
using Finance.Application.UseCases.Budgets.UpdateBudget.Request;
using Finance.Application.UseCases.Budgets.UpdateBudget.Response;
using Finance.Domain;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsByUserId
{

    public class GetBudgetsByUserIdUseCase
    {
        private readonly IBudgetRepository _budget;
        private readonly ILogger<GetBudgetsByUserIdUseCase> _logger;
        public GetBudgetsByUserIdUseCase(IBudgetRepository budget, ILogger<GetBudgetsByUserIdUseCase> logger)
        {
            _budget = budget;
            _logger = logger;
        }
        public async Task<GetBudgetsByUserIdResponse> ExecuteAsync(GetBudgetsByUserIdRequest request, CancellationToken ct)
        {
            try
            {
                var budgets = await _budget.GetBudgetsByUserId(request.UserId, ct);
                var result = budgets.Select(x => new BudgetDto
                {
                    BudgetId = x.BudgetId,
                    Name = x.Name,
                    LimitAmount = x.LimitAmount,
                    Date = x.Date,
                    CategoryId = x.CategoryId,
                    CategoryName=x.Category.Name
                }).ToList();
                return new GetBudgetsByUserIdSuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetBudgetsByUserIdErrorResponse("Unable to get Budgets at this time", "INVALID_UPDATE");
            }
        }
    }
}
