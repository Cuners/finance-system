using Finance.Application.UseCases.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Categories.GetCategories.Response
{
    public class GetCategoriesSuccessResponse : GetCategoriesResponse
    {
        public IEnumerable<CategoryDto> Categories { get; }

        public GetCategoriesSuccessResponse(IEnumerable<CategoryDto> categories)
        {
            Categories = categories;
        }
    }
}
