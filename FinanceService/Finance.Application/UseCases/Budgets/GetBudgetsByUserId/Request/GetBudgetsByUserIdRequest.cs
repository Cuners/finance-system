using Finance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.GetBudgetsByUserId.Request
{
    public class GetBudgetsByUserIdRequest
    {
        public int UserId {  get; set; }
    }
}
