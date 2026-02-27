using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.DeleteTransaction.Response
{
    public class DeleteTransactionSuccessRepsonse : DeleteTransactionResponse
    {
        public int TransactionId { get; set; }
        public DeleteTransactionSuccessRepsonse(int id)
        {
            TransactionId = id;
        }
    }
}
