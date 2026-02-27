using Finance.Application.UseCases.Categories.GetCategories.Request;
using Finance.Application.UseCases.Categories.GetCategories.Response;
using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Categories.GetCategories
{
    public class GetCategoriesUseCase
    {
        private readonly ICategoryRepository _categories;
        private readonly ILogger<GetCategoriesUseCase> _logger;
        public GetCategoriesUseCase(ICategoryRepository categories, ILogger<GetCategoriesUseCase> logger)
        {
            _categories = categories;
            _logger = logger;
        }
        public async Task<GetCategoriesResponse> ExecuteAsync(CancellationToken ct)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetCategoriesErrorResponse("Unable to get Categories at this time", "INVALID_CATEGORIES");
            }
        }

    }
}
