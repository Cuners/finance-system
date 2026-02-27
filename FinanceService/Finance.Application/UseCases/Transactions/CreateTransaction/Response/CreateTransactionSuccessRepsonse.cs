using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.CreateTransaction.Response
{
    public class CreateTransactionSuccessRepsonse : CreateTransactionResponse
    {
        public int TransactionId { get; set; }
        public CreateTransactionSuccessRepsonse(int id)
        {
            TransactionId = id;
        }
    }
}
