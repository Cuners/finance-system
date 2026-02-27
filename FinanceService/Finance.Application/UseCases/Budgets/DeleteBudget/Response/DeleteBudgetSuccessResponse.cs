using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.DeleteBudget.Response
{
    public class DeleteBudgetSuccessResponse : DeleteBudgetResponse
    {
        public int BudgetId {  get; set; }
        public DeleteBudgetSuccessResponse(int id)
        {
            BudgetId = id;
        }
    }
}
