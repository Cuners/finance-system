using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactions.Response
{
    public class GetTransactionsErrorResponse : GetTransactionsResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetTransactionsErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
