using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LogoutUser.Response
{
    public class LogoutErrorResponse : LogoutResponse
    {
        public string Message { get; }
        public string Code { get; }
        public LogoutErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
