using Finance.Application.Common;
using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Categories.GetCategories
{
    public class GetCategoriesUseCase : IGetCategoriesUseCase
    {
        private readonly ICategoryRepository _categories;
        private readonly ILogger<GetCategoriesUseCase> _logger;
        private readonly ICacheService _cache;

        public GetCategoriesUseCase(
            ICategoryRepository categories,
            ILogger<GetCategoriesUseCase> logger,
            ICacheService cache)
        {
            _categories = categories;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<GetCategoriesResult>> ExecuteAsync(
            GetCategoriesQuery query,
            int userId,
            CancellationToken ct)
        {
            const string cacheKey = "categories";

            try
            {
                var categories = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    return await BuildCategoryDtosAsync(token);
                });

                return Result<GetCategoriesResult>.Success(
                    new GetCategoriesResult(categories ?? []));
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, "Cache timeout while getting categories");
                var categories = await BuildCategoryDtosAsync(ct);
                return Result<GetCategoriesResult>.Success(new GetCategoriesResult(categories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get categories");
                return Result<GetCategoriesResult>.Failure(
                    "CATEGORIES_GET_FAILED",
                    "Unable to get categories at this time");
            }
        }

        private async Task<IReadOnlyList<CategoryDto>> BuildCategoryDtosAsync(CancellationToken ct)
        {
            var categories = await _categories.GetAllCategories(ct);
            if (categories == null)
            {
                return [];
            }

            return categories.Select(x => new CategoryDto
            {
                CategoryId = x.CategoryId,
                Name = x.Name
            }).ToList();
        }
    }
}
