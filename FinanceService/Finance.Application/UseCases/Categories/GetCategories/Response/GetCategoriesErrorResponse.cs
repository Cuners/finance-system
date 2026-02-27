using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Categories.GetCategories.Response
{
    public class GetCategoriesErrorResponse : GetCategoriesResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetCategoriesErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }

    }
}
