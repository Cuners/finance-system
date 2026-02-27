using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactionById.Response
{
    public class GetTransactionByIdSuccessResponse : GetTransactionByIdResponse
    {
        public TransactionDto Transaction { get; set; }
        public GetTransactionByIdSuccessResponse(TransactionDto transaction)
        {
            Transaction=transaction;
        }
    }
}
