using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain
{
    public partial class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    }
}
