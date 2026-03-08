using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain.Queries
{
    public record TransactionFilter(
        int UserId,
        int? AccountId,
        string? Type = null,       
        DateOnly? StartDate = null,
        DateOnly? EndDate = null,
        string? SortBy = null,
        string? SortOrder=null,
        int Page = 1,
        int PageSize = 20
    );
}
