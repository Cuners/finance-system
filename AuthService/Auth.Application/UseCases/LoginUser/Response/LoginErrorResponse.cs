using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LoginUser.Response
{
    public class LoginErrorResponse : LoginRepsonse
    {
        public string Message { get; }
        public string Code { get; }
        public LoginErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
