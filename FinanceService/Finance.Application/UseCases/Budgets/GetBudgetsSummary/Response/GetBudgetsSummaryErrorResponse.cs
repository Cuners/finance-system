using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsSummary.Response
{
    public class GetBudgetsSummaryErrorResponse : GetBudgetsSummaryResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetBudgetsSummaryErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
