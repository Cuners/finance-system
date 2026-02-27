using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsByUserId.Response
{
    public class GetBudgetsByUserIdErrorResponse : GetBudgetsByUserIdResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetBudgetsByUserIdErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
