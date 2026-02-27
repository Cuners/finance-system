using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetById.Response
{
    public class GetBudgetByIdSuccessResponse : GetBudgetByIdResponse
    {
        public BudgetDto Budget { get; set; }
        public GetBudgetByIdSuccessResponse(BudgetDto budget)
        {
            Budget = budget;
        }
    }
}
