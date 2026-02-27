
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactionsByAccountId.Response
{
    public class GetTransactionsByAccountIdErrorResponse : GetTransactionsByAccountIdResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetTransactionsByAccountIdErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
