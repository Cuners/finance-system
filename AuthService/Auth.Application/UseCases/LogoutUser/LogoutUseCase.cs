using Auth.Application.Services;
using Auth.Application.UseCases.LogoutUser.Request;
using Auth.Application.UseCases.LogoutUser.Response;
using Auth.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LogoutUser
{
    public class LogoutUseCase : IUseCase<LogoutRequest, LogoutResponse>
    {
        private readonly IRefreshTokenRepository _refreshToken;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LogoutUseCase> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public LogoutUseCase(IRefreshTokenRepository refreshToken,
                             ITokenService tokenService,
                             ILogger<LogoutUseCase> logger,
                             IUnitOfWork unitOfWork)
        {
            _refreshToken = refreshToken;
            _tokenService = tokenService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<LogoutResponse> ExecuteAsync(LogoutRequest request, CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrEmpty(request.RefreshToken))
                {
                    return new LogoutSuccessRepsonse();
                }
                var jti = _tokenService.GetJtiFromToken(request.RefreshToken);
                if (string.IsNullOrEmpty(jti))
                {
                    return new LogoutErrorResponse("Invalid token format", "INVALID_TOKEN");
                }
                var storedToken = await _refreshToken.GetTokenByJti(jti, ct);
                if (storedToken != null && !storedToken.IsRevoked)
                {
                    storedToken.IsRevoked = true;
                    storedToken.RevokedAt = DateTime.UtcNow;
                    await _unitOfWork.SaveChangesAsync(ct);

                    _logger.LogInformation($"Refresh token {jti} revoked for user {storedToken.UserId}");
                }
                return new LogoutSuccessRepsonse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return new LogoutErrorResponse("Logout failed", "LOGOUT_ERROR");
            }
        }
    }
}
