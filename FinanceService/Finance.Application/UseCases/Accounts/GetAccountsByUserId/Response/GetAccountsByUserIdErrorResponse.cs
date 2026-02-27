using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetAccountsByUserId.Response
{
    public class GetAccountsByUserIdErrorResponse : GetAccountsByUserIdResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetAccountsByUserIdErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
