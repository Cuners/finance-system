using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain
{
    public record FinancialSummary(
        decimal TotalIncome,
        decimal TotalExpenses,
        decimal NetChange
    );
}
