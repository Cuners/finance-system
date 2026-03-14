using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.UpdateTransaction.Response
{
    public class UpdateTransactionSuccessResponse : UpdateTransactionResponse
    {
        public int TransactionId { get; }
        public UpdateTransactionSuccessResponse(int id)
        {
            TransactionId = id;
        }
    }
}
