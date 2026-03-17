using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.RefreshToken.Request
{
    public record RefreshTokenRequest(string RefreshToken);
}
