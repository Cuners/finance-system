using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.RegistrateUser.Response
{
    public class RegistrationErrorResponse : RegistrationResponse
    {
        public string Message {  get; }
        public string Code {  get; }
        public RegistrationErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
