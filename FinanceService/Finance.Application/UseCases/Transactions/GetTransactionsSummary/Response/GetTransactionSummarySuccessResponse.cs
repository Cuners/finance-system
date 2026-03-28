using Finance.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactionsSummary.Response
{
    public class GetTransactionSummarySuccessResponse : GetTransactionSummaryResponse
    {
        public TransactionSummaryDto Transactions { get; set; }
        public GetTransactionSummarySuccessResponse(TransactionSummaryDto transactions)
        {
            Transactions = transactions;
        }
    }
}
