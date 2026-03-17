using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LogoutUser.Request
{
    public record LogoutRequest(string? RefreshToken);
}
