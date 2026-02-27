using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.UpdateBudget.Response
{
    public class UpdateBudgetSuccessResponse : UpdateBudgetResponse
    {
        public int BudgetId {  get; set; }
        public UpdateBudgetSuccessResponse(int id)
        {
            BudgetId = id;
        }
    }
}
