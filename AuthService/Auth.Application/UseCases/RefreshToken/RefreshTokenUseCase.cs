using Auth.Application.Services;
using Auth.Application.UseCases.RefreshToken.Request;
using Auth.Application.UseCases.RefreshToken.Response;
using Auth.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Application.UseCases.RefreshToken
{
    public class RefreshTokenUseCase : IUseCase<RefreshTokenRequest, RefreshTokenResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RefreshTokenUseCase(IRefreshTokenRepository refreshTokenRepository,
                            ITokenService tokenService,
                            IUserRepository userRepository,
                            IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository=refreshTokenRepository;
            _tokenService=tokenService;
            _userRepository=userRepository;
            _unitOfWork=unitOfWork;
        }
        public async Task<RefreshTokenResponse> ExecuteAsync(RefreshTokenRequest request, CancellationToken ct)
        {
            var refreshToken=request.RefreshToken;
            var principal = _tokenService.ValidateToken(refreshToken);
            if (principal == null)
            {
                return new RefreshTokenErrorResponse("Invalid token", "INVALID_TOKEN");
            }
            var jti = _tokenService.GetJtiFromToken(refreshToken);
            var userId = _tokenService.GetUserIdFromToken(refreshToken);
            if (string.IsNullOrEmpty(jti) || userId == null)
            {
                return new RefreshTokenErrorResponse("Invalid claims", "INVALID_CLAIMS");
            }
            var storedToken = await _refreshTokenRepository.GetTokenByJti(jti, ct);

            if (storedToken == null || storedToken.IsRevoked)
            {
                return new RefreshTokenErrorResponse("Token revoked", "REVOKED");
            }
            if (storedToken.ExpiresAt < DateTime.UtcNow)
            {
                await RevokeTokenAsync(storedToken, ct);
                return new RefreshTokenErrorResponse("Token expired", "EXPIRED");
            }
            await RevokeTokenAsync(storedToken, ct);
            var user = await _userRepository.GetUserById(userId.Value, ct);
            if (user == null)
            {
                return new RefreshTokenErrorResponse("User not found", "USER_NOT_FOUND");
            }
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshTokenString = _tokenService.GenerateRefreshToken(user);
            var newJti = _tokenService.GetJtiFromToken(newRefreshTokenString);

            var refreshTokenToDb = new Auth.Domain.RefreshToken()
            {
                UserId = user.UserId,
                Jti = newJti,
                TokenHash = HashToken(newRefreshTokenString),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false,
            };
            await _refreshTokenRepository.Create(refreshTokenToDb, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new RefreshTokenSuccessResponse(newAccessToken, newRefreshTokenString);
        }

        private async Task RevokeTokenAsync(Auth.Domain.RefreshToken token, CancellationToken ct)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            await Task.CompletedTask;
        }

        private string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hash);
        }
    }
}
