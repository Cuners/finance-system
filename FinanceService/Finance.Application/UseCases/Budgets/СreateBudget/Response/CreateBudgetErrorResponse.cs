using Finance.Application.UseCases.Accounts.CreateAccount.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.СreateBudget.Response
{
    public class CreateBudgetErrorResponse : CreateBudgetResponse
    {
        public string Message { get; }
        public string Code { get; }
        public CreateBudgetErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
