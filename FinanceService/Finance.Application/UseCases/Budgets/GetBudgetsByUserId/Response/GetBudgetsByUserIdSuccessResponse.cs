using Finance.Application.UseCases.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsByUserId.Response
{
    public class GetBudgetsByUserIdSuccessResponse : GetBudgetsByUserIdResponse
    {
        public IEnumerable<BudgetDto> Budgets { get; }

        public GetBudgetsByUserIdSuccessResponse(IEnumerable<BudgetDto> budgets)
        {
            Budgets = budgets;
        }
    }
}
