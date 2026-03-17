using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.RefreshToken.Response
{
    public class RefreshTokenSuccessResponse: RefreshTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public RefreshTokenSuccessResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
