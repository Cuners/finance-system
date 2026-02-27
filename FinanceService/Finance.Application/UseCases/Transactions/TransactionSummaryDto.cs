using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions
{
    public record TransactionSummaryDto(
        decimal TotalIncome,
        decimal TotalExpenses,
        decimal NetBalance);
}
