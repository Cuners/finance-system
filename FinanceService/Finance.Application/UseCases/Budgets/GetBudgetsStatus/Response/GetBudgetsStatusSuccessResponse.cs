using Finance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsStatus.Response
{
    public class GetBudgetsStatusSuccessResponse : GetBudgetsStatusResponse
    {
        public IEnumerable<BudgetStatus> BudgetStatuses { get; set; }
        public GetBudgetsStatusSuccessResponse(IEnumerable<BudgetStatus> budgetStatuses)
        {
            BudgetStatuses = budgetStatuses;
        }
    }
}
