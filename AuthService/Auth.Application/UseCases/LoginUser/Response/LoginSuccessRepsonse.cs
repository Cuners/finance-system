using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LoginUser.Response
{
    public class LoginSuccessRepsonse : LoginRepsonse
    {
        public string AccessToken {  get; set; }
        public string RefreshToken { get; set; }
        public LoginSuccessRepsonse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
