using Finance.Application.Common;

namespace Finance.Application.UseCases.Categories.GetCategories
{
    public interface IGetCategoriesUseCase
    {
        Task<Result<GetCategoriesResult>> ExecuteAsync(
            GetCategoriesQuery query,
            int userId,
            CancellationToken ct);
    }
}
