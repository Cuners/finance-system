
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.GetUsers.Response
{
    public class UsersErrorResponse : UsersResponse
    {
        public string Message { get; }
        public string Code { get; }
        public UsersErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
