using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.UpdateTransaction.Request
{
    public class UpdateTransactionRequest
    {
        public int TransactionId {  get; set; }
        public int AccountId { get; set; }

        public int CategoryId { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }

        public string? Note { get; set; }
    }
}
