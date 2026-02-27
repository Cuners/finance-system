using Finance.Application.UseCases.Accounts.DeleteAccount.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.DeleteBudget.Response
{
    public class DeleteBudgetErrorResponse : DeleteBudgetResponse
    {
        public string Message { get; }
        public string Code { get; }
        public DeleteBudgetErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
