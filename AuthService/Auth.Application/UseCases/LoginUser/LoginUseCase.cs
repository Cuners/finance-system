using Auth.Application.Services;
using Auth.Application.UseCases.LoginUser.Request;
using Auth.Application.UseCases.LoginUser.Response;
using Auth.Domain;
using Auth.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LoginUser
{
    public class LoginUseCase : IUseCase<LoginRequest,LoginRepsonse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginUseCase> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LoginUseCase(IUserRepository userRepository, 
                            IPasswordService passwordService, 
                            ITokenService tokenService, 
                            ILogger<LoginUseCase> logger, 
                            IRefreshTokenRepository refreshTokenRepository,
                            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _logger = logger;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork=unitOfWork;
        }

        public async Task<LoginRepsonse> ExecuteAsync(LoginRequest request, CancellationToken ct)
        {
            try
            {

                var user = await _userRepository.GetUserByName(request.Username, ct);

                if (user == null)
                {
                    return new LoginErrorResponse("Invalid Login", "INVALID_Login"); ;
                }
                //if (!_passwordService.Verify(request.Password, user.PassHash))
                //{
                //    return new LoginErrorResponse("Invalid credentials", "INVALID_CREDENTIALS");
                //}
                var accessToken = _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken(user);
                var jti = _tokenService.GetJtiFromToken(refreshToken);
                var expiresAt = _tokenService.GetExpirationFromToken(refreshToken);
                if (string.IsNullOrEmpty(jti))
                {
                    _logger.LogError($"Failed to extract JTI from refresh token for user {user.UserId}");
                    return new LoginErrorResponse("Token generation failed", "TOKEN_ERROR");
                }
                var refreshTokenToDb = new Auth.Domain.RefreshToken()
                {
                    UserId = user.UserId,
                    Jti = jti,
                    TokenHash = HashToken(refreshToken),
                    ExpiresAt = expiresAt,
                    CreatedAt = DateTime.UtcNow,
                    IsRevoked = false,
                };
                await _refreshTokenRepository.Create(refreshTokenToDb,ct);
                await _unitOfWork.SaveChangesAsync(ct);
                return new LoginSuccessRepsonse(accessToken, refreshToken);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new LoginErrorResponse("Unable to login at this time", "INVALID_LOGIN");
            }
        }
        private string HashToken(string token)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hash);
        }
    }
}
