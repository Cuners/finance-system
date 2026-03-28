using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.DTO
{
    public record BudgetSummaryDto(
        decimal TotalBudget,
        decimal TotalSpent,
        decimal Remaining,
        decimal ProcentSpent);
    
}
