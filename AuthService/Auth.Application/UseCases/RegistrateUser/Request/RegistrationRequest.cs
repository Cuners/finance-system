using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.RegistrateUser.Request
{
    public record RegistrationRequest(string Login,
                                      string Password,
                                      string Email);
}
