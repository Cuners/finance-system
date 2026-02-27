using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactionsByAccountId.Response
{
    public class GetTransactionsByAccountIdSuccessResponse : GetTransactionsByAccountIdResponse
    {
        public IEnumerable<TransactionDto> Transactions { get; set; }
        public GetTransactionsByAccountIdSuccessResponse(IEnumerable<TransactionDto> transactions) 
        {
            Transactions = transactions;
        } 

    }
}
