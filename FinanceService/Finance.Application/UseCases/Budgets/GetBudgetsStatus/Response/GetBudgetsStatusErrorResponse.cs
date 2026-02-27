using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsStatus.Response
{
    public class GetBudgetsStatusErrorResponse : GetBudgetsStatusResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetBudgetsStatusErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
