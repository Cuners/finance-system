using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.UpdateTransaction.Response
{
    public class UpdateTransactionErrorResponse : UpdateTransactionResponse
    {
        public string Message { get; }
        public string Code { get; }
        public UpdateTransactionErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
