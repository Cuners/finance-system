using Finance.Application.DTO;

namespace Finance.Application.UseCases.Categories.GetCategories
{
    public record GetCategoriesResult(IReadOnlyList<CategoryDto> Categories);
}
