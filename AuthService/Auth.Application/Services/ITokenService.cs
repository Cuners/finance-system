using Auth.Domain;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Auth.Application.Services
{
    public interface ITokenService
    {
        List<Claim> GenerateUserClaims(User user);

        string GenerateAccessToken(User user);
        string GenerateRefreshToken(User user);
        string? GetJtiFromToken(string token);
        DateTime GetExpirationFromToken(string token);
        int? GetUserIdFromToken(string token);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
