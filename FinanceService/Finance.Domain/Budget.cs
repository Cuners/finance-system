using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain
{
    public partial class Budget
    {
        public int BudgetId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public decimal LimitAmount { get; set; }

        public DateOnly Date { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
    }
}
