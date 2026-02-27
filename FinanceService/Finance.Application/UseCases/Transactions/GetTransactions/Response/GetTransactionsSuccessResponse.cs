using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactions.Response
{
    public class GetTransactionsSuccessResponse : GetTransactionsResponse
    {
        public IEnumerable<TransactionDto> Transactions { get; set; }
        public GetTransactionsSuccessResponse(IEnumerable<TransactionDto> transactions)
        {
            Transactions = transactions;
        }
    }
}
