using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.RegistrateUser.Request
{
    public class RegistrationRequest
    {
        public string Login { get; set; }
        public string Password{ get; set; }
        public string Email { get; set; }
    }
}
