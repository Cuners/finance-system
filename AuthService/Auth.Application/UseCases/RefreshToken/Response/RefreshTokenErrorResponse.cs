using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.RefreshToken.Response
{
    public class RefreshTokenErrorResponse : RefreshTokenResponse
    {
        public string Message { get; }
        public string Code { get; }
        public RefreshTokenErrorResponse(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
