using Auth.Application.UseCases.LogoutUser.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LogoutUser
{
    public class LogoutUseCase
    {
        public LogoutResponse Execute()
        {
            return new LogoutSuccessRepsonse();
        }
    }
}
