using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.СreateBudget.Response
{
    public class CreateBudgetSuccessResponse : CreateBudgetResponse
    {
        public int BudgetId {  get; set; }
        public CreateBudgetSuccessResponse(int id) 
        { 
            BudgetId = id;
        }
    }
}
