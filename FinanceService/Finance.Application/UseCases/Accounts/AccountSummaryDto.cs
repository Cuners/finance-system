using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts
{
    public class AccountSummaryDto
    {
        public int AccountId { get; init; }
        public string Name { get; init; } = "";
        public decimal Balance { get; init; }
        public string? Note { get; init; }
        public decimal Income { get; init; }
        public decimal Expense { get; init; }
        public int Count { get; init; }
    }
}
