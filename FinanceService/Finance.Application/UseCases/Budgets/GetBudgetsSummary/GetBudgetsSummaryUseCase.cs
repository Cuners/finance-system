using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Application.UseCases.Budgets.GetBudgetsSummary.Request;
using Finance.Application.UseCases.Budgets.GetBudgetsSummary.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsSummary
{
    public class GetBudgetsSummaryUseCase : IUseCase<GetBudgetsSummaryRequest, GetBudgetsSummaryResponse>
    {
        private readonly IBudgetRepository _BudgetRepository;
        private readonly ILogger<GetBudgetsSummaryUseCase> _logger;
        private readonly ICacheService _cache;
        public GetBudgetsSummaryUseCase(IBudgetRepository BudgetRepository, ILogger<GetBudgetsSummaryUseCase> logger, ICacheService cache)
        {
            _BudgetRepository = BudgetRepository;
            _logger = logger;
            _cache = cache;
        }
        public async Task<GetBudgetsSummaryResponse> ExecuteAsync(GetBudgetsSummaryRequest request,int userId, CancellationToken ct)
        {
            //if (request.UserId <= 0)
            //{
            //    _logger.LogWarning("GetBudgetRequest is null");
            //    return new GetBudgetsSummaryErrorResponse("Invalid User id", "INVALID_USER_ID");
            //}
            var cacheKey = $"dashboard:user:{userId}:" + "budgetsSummary";
            try
            {
                var budgets = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    var budgets = await _BudgetRepository.GetBudgetStatusAsync(userId, ct);
                    var totalBudget = budgets.Sum(b => b.LimitAmount);
                    var totalSpent = budgets.Sum(b => b.TotalSpent);
                    var remaining = totalBudget - totalSpent;
                    var procentSpent = Math.Round(totalSpent / (totalBudget / 100));
                    var sumBudget = new BudgetSummaryDto(TotalBudget: totalBudget,
                                                         TotalSpent: totalSpent,
                                                         ProcentSpent: procentSpent,
                                                         Remaining: remaining);
                    return sumBudget;
                });
                if (budgets is null)
                {
                    _logger.LogWarning("Summary of budgets is null");
                    return new GetBudgetsSummaryErrorResponse("No Budget found", "Budget_NOT_FOUND");
                }
                return new GetBudgetsSummarySuccessResponse(budgets);
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, $"Cache lock timeout for key {cacheKey}");
                var budgets = await _BudgetRepository.GetBudgetStatusAsync(userId, ct);
                var totalBudget = budgets.Sum(b => b.LimitAmount);
                var totalSpent = budgets.Sum(b => b.TotalSpent);
                var remaining = totalBudget - totalSpent;
                var procentSpent = Math.Round(totalSpent / (totalBudget / 100));
                var sumBudget = new BudgetSummaryDto(TotalBudget: totalBudget,
                                                     TotalSpent: totalSpent,
                                                     ProcentSpent: procentSpent,
                                                     Remaining: remaining);
                return new GetBudgetsSummarySuccessResponse(sumBudget);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetBudgetsSummaryErrorResponse("Unable to get Budget at this time", "INVALID_GET");
            }
           
        }
    }
}
