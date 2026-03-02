using Finance.Application.Services;
using Finance.Application.UseCases.Budgets.GetBudgetsStatus.Response;
using Finance.Application.UseCases.Categories.GetCategories.Request;
using Finance.Application.UseCases.Categories.GetCategories.Response;
using Finance.Application.UseCases.Transactions.GetTransactions.Response;
using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId.Response;
using Finance.Domain;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Finance.Application.UseCases.Categories.GetCategories
{
    public class GetCategoriesUseCase
    {
        private readonly ICategoryRepository _categories;
        private readonly ILogger<GetCategoriesUseCase> _logger;
        private readonly ICacheService _cache;
        public GetCategoriesUseCase(ICategoryRepository categories, ILogger<GetCategoriesUseCase> logger, ICacheService cache)
        {
            _categories = categories;
            _logger = logger;
            _cache = cache;
        }
        public async Task<GetCategoriesResponse> ExecuteAsync(CancellationToken ct)
        {
            var cacheKey = $"categories";
            try
            {
                var categories = await _categories.GetAllCategories(ct);
                if (!categories.Any() || categories == null)
                {
                    _logger.LogError("Categories is null");
                    return new GetCategoriesErrorResponse("Invalid Categories", "Invalid Category");
                }
                var budgets = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    var categories = await _categories.GetAllCategories(ct);
                    var result = categories.Select(x => new CategoryDto
                    {
                        Name = x.Name
                    });
                    return result;
                });
                if (budgets is null)
                {
                    _logger.LogError("Categories is null");
                    return new GetCategoriesErrorResponse("Invalid Categories", "Invalid Category");
                }
                return new GetCategoriesSuccessResponse(budgets);
            }
            catch (TimeoutException ex)
            {
                var categories = await _categories.GetAllCategories(ct);
                if (!categories.Any() || categories == null)
                {
                    _logger.LogError("Categories is null");
                    return new GetCategoriesErrorResponse("Invalid Categories", "Invalid Category");
                }
                var result = categories.Select(x => new CategoryDto
                {
                    Name = x.Name
                });
                return new GetCategoriesSuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetCategoriesErrorResponse("Unable to get Categories at this time", "INVALID_CATEGORIES");
            }
        }

    }
}
