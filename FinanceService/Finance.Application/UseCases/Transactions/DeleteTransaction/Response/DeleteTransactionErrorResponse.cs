using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.DeleteTransaction.Response
{
    public class DeleteTransactionErrorResponse : DeleteTransactionResponse
    {
        public string Message { get; }
        public string Code { get; }
        public DeleteTransactionErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
