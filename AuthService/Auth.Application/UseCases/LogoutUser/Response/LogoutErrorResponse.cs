using Auth.Application.UseCases.LoginUser.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LogoutUser.Response
{
    public class LogoutErrorResponse : LoginRepsonse
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
