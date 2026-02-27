using Finance.Application.UseCases.Accounts.DeleteAccount.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.DeleteAccount.Response
{
    public class DeleteAccountErrorResponse : DeleteAccountResponse
    {
        public string Message { get; }
        public string Code { get; }
        public DeleteAccountErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
