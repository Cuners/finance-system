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

        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken(IEnumerable<Claim> claims);

        (string AccessToken, string RefreshToken) RefreshTokens(string oldRefreshToken);
    }
}
