using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactions.Request
{
    public record GetTransactionsRequest(
        int? AccountId,
        string? Type = null,
        string? SortOrder=null,
        string? SortBy=null,
        DateOnly? StartDate = null,
        DateOnly? EndDate = null
    );
}
