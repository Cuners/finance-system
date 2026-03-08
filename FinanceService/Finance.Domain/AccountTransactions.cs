using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain
{
    public record AccountTransactions(int AccountId,
                                      decimal Income,
                                      decimal Expense,
                                      int Count);
}
