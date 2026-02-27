using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain
{
    public record BudgetStatus(
        int BudgetId,
        string CategoryName,
        decimal LimitAmount,
        decimal TotalSpent,
        decimal ProcentSpent,
        decimal Remaining,
        DateOnly Period 
    );
}
