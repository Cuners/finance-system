using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.UpdateBudget.Response
{
    public class UpdateBudgetErrorResponse: UpdateBudgetResponse
    {
        public string Message { get; }
        public string Code { get; }
        public UpdateBudgetErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
