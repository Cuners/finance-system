using Finance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.UpdateBudget.Request
{
    public class UpdateBudgetRequest
    {
        public int BudgetId { get; set; }

        public string Name { get; set; } = null!;

        public decimal LimitAmount { get; set; }
        public int CategoryId { get; set; }

        public DateOnly Date { get; set; }

        
    }
}
