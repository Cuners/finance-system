using Auth.Application.Services;
using Auth.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;

            // Валидация настроек
            if (string.IsNullOrWhiteSpace(_jwtSettings.SecretKey))
                throw new InvalidOperationException("JWT SecretKey is not configured.");
            if (_jwtSettings.SecretKey.Length < 32)
                throw new InvalidOperationException("JWT SecretKey must be at least 32 characters long.");
        }

        public List<Claim> GenerateUserClaims(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, user.Login),
                new("user_id",user.UserId.ToString())
            };
            foreach (var role in user.Roles)
            {
                if (!string.IsNullOrWhiteSpace(role.RoleName))
                    claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                foreach(var permission in role.Permissions)
                {
                    claims.Add(new Claim("permission", permission.PermissionName));
                }
            }
            return claims;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            return CreateToken(claims, TimeSpan.FromMinutes(_jwtSettings.AccessTokenExpiryMinutes));
        }

        public string GenerateRefreshToken(IEnumerable<Claim> claims)
        {
            return CreateToken(claims, TimeSpan.FromDays(_jwtSettings.RefreshTokenExpiryDays));
        }

        public (string AccessToken, string RefreshToken) RefreshTokens(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("Refresh token is required.", nameof(refreshToken));

            var handler = new JwtSecurityTokenHandler();

            // Валидируем refresh token
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
            };

            try
            {
                handler.ValidateToken(refreshToken, validationParameters, out _);
                var jwtToken = handler.ReadJwtToken(refreshToken);
                var claims = jwtToken.Claims.ToList();

                var accessToken = GenerateAccessToken(claims);
                var newRefreshToken = GenerateRefreshToken(claims);

                return (accessToken, newRefreshToken);
            }
            catch (SecurityTokenException ex)
            {
                throw new InvalidOperationException("Invalid refresh token.", ex);
            }
        }

        private string CreateToken(IEnumerable<Claim> claims, TimeSpan expiry)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(expiry),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
