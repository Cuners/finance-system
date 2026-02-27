using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Budgets
{
    public record BudgetSummaryDto(
        decimal TotalBudget,
        decimal TotalSpent,
        decimal Remaining,
        decimal ProcentSpent);
    
}
