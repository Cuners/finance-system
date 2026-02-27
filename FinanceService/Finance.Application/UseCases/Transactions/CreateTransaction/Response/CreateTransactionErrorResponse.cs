using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.CreateTransaction.Response
{
    public class CreateTransactionErrorResponse : CreateTransactionResponse
    {
        public string Message { get; }
        public string Code { get; }
        public CreateTransactionErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
