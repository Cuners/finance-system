using Finance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.СreateBudget.Request
{
    public class CreateBudgetRequest
    {

        public string Name { get; set; } = null!;

        public decimal LimitAmount { get; set; }

        public int CategoryId { get; set; }
    }
}
