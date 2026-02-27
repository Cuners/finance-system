using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.RegistrateUser.Response
{
    public class RegistrationSuccessResponse : RegistrationResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public RegistrationSuccessResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
