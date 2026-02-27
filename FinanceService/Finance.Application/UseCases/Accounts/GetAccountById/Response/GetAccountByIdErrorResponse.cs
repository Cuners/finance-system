using Finance.Application.UseCases.Accounts.GetAccountById.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetAccountById.Response
{
    public class GetAccountByIdErrorResponse : GetAccountByIdResponse
    {
        public string Message { get; }
        public string Code { get; }
        public GetAccountByIdErrorResponse(string message, string code)
        {
            Message=message;
            Code=code;
        }
    }
}
