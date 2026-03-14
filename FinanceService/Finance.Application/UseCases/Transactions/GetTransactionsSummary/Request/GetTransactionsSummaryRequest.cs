using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactionsSummary.Request
{
    public class GetTransactionsSummaryRequest
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
