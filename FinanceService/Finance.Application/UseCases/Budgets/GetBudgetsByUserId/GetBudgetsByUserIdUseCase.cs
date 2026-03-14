using Finance.Application.Services;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId.Request;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsByUserId
{

    public class GetBudgetsByUserIdUseCase : IUseCase<GetBudgetsByUserIdRequest, GetBudgetsByUserIdResponse>
    {
        private readonly IBudgetRepository _budget;
        private readonly ILogger<GetBudgetsByUserIdUseCase> _logger;
        private readonly ICacheService _cache;
        public GetBudgetsByUserIdUseCase(IBudgetRepository budget,
                                         ILogger<GetBudgetsByUserIdUseCase> logger,
                                         ICacheService cache)
        {
            _budget = budget;
            _logger = logger;
            _cache = cache;
        }
        public async Task<GetBudgetsByUserIdResponse> ExecuteAsync(GetBudgetsByUserIdRequest request, int userId, CancellationToken ct)
        {
            var cacheKey = $"budgets:user:{userId}:" + "byId";
            try
            {
                var budgets = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    var budgets = await _budget.GetBudgetsByUserId(userId, ct);
                    var result = budgets.Select(x => new BudgetDto
                    {
                        BudgetId = x.BudgetId,
                        Name = x.Name,
                        LimitAmount = x.LimitAmount,
                        Date = x.Date,
                        CategoryId = x.CategoryId,
                        CategoryName = x.Category.Name
                    }).ToList();
                    return result;
                });
                if(budgets is null)
                {
                    _logger.LogWarning("BudgetsUserId is null");
                    return new GetBudgetsByUserIdErrorResponse("No Budget found", "Budget_NOT_FOUND");
                }
                return new GetBudgetsByUserIdSuccessResponse(budgets);
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, $"Cache lock timeout for key {cacheKey}");
                var budgets = await _budget.GetBudgetsByUserId(userId, ct);
                var result = budgets.Select(x => new BudgetDto
                {
                    BudgetId = x.BudgetId,
                    Name = x.Name,
                    LimitAmount = x.LimitAmount,
                    Date = x.Date,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name
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
