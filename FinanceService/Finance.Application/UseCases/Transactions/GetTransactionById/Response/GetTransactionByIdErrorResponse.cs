using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactionById.Response
{
    public class GetTransactionByIdErrorResponse : GetTransactionByIdResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetTransactionByIdErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
