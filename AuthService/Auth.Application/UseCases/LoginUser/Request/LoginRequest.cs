using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LoginUser.Request
{
    public class LoginRequest
    {
        public string Username { get; set; } // Логин пользователя
        public string Password { get; set; } // Пароль пользователя
    }
}
