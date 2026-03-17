using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LoginUser.Request
{
    public record LoginRequest(string Username, 
                               string Password);
}
