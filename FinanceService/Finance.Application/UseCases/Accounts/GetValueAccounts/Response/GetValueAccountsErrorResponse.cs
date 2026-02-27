using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetValueAccounts.Response
{
    public class GetValueAccountsErrorResponse : GetValueAccountsResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetValueAccountsErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
