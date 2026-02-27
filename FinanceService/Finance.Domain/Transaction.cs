using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }

        public int AccountId { get; set; }

        public int CategoryId { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }

        public string? Note { get; set; }

        public virtual Account Account { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;
    }
}
