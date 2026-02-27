
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.UpdateAccount.Response
{
    public class UpdateAccountErrorResponse : UpdateAccountResponse
    {
        public string Message { get; }
        public string Code { get; }
        public UpdateAccountErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
