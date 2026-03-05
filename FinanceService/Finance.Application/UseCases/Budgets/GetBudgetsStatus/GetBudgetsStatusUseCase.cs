using Finance.Application.Services;
using Finance.Application.UseCases.Budgets.GetBudgetsStatus.Request;
using Finance.Application.UseCases.Budgets.GetBudgetsStatus.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsStatus
{
    public class GetBudgetsStatusUseCase : IUseCase<GetBudgetsStatusRequest, GetBudgetsStatusResponse>
    {
        private readonly IBudgetRepository _BudgetRepository;
        private readonly ILogger<GetBudgetsStatusUseCase> _logger;
        private readonly ICacheService _cache;
        public GetBudgetsStatusUseCase(IBudgetRepository BudgetRepository, ILogger<GetBudgetsStatusUseCase> logger, ICacheService cache)
        {
            _BudgetRepository = BudgetRepository;
            _logger = logger;
            _cache = cache;
        }
        public async Task<GetBudgetsStatusResponse> ExecuteAsync(GetBudgetsStatusRequest request, CancellationToken ct)
        {
            if (request.UserId <= 0)
            {
                _logger.LogWarning("GetBudgetRequest is null");
                return new GetBudgetsStatusErrorResponse("Invalid User id", "INVALID_USER_ID");
            }
            var cacheKey = $"dashboard:user:{request.UserId}:" + "budgetsStatus";
            try
            {
                var budgets = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    var summary = await _BudgetRepository.GetBudgetStatusAsync(request.UserId,ct);
                    return summary;
                });
               

                if (budgets is null)
                {
                    _logger.LogWarning("Status of budgets is null");
                    return new GetBudgetsStatusErrorResponse("No Budget found", "Budget_NOT_FOUND");
                }
                return new GetBudgetsStatusSuccessResponse(budgets);
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, $"Cache lock timeout for key {cacheKey}");
                var summary = await _BudgetRepository.GetBudgetStatusAsync(request.UserId, ct);
                return new GetBudgetsStatusSuccessResponse(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении статуса бюджетов для пользователя {request.UserId}");
                return new GetBudgetsStatusErrorResponse("Unable to get Budget at this time", "INVALID_GET");
            }
        }
    }
}
