using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.CreateAccount.Response
{
    public class CreateAccountErrorResponse : CreateAccountResponse
    {
        public string Message { get; }
        public string Code { get; }
        public CreateAccountErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
