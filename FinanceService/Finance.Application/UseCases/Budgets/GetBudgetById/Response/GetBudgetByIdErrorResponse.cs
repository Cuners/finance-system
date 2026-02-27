using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetById.Response
{
    public class GetBudgetByIdErrorResponse : GetBudgetByIdResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetBudgetByIdErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }

    }
}
