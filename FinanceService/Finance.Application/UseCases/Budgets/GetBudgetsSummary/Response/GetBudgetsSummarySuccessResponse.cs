
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsSummary.Response
{
    public class GetBudgetsSummarySuccessResponse : GetBudgetsSummaryResponse
    {
        public BudgetSummaryDto BudgetSummaryDto { get; set; }
        public GetBudgetsSummarySuccessResponse(BudgetSummaryDto budgetSummaryDto )
        {
            BudgetSummaryDto = budgetSummaryDto;
        }
    }
}
