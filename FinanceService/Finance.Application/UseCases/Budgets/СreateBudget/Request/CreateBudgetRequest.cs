using Finance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets.СreateBudget.Request
{
    public class CreateBudgetRequest
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public decimal LimitAmount { get; set; }

        public DateOnly Date { get; set; }

        public int CategoryId { get; set; }
    }
}
