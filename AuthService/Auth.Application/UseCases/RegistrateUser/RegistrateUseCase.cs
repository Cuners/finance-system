using Auth.Application.Services;
using Auth.Application.UseCases.RegistrateUser.Request;
using Auth.Application.UseCases.RegistrateUser.Response;
using Auth.Domain;
using Auth.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Auth.Application.UseCases.RegistrateUser
{
    public class RegistrateUseCase : IUseCase<RegistrationRequest,RegistrationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegistrateUseCase> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RegistrateUseCase(IUserRepository userRepository, 
                                 IUnitOfWork unitOfWork, 
                                 IPasswordService passwordService, 
                                 ITokenService tokenService,
                                 IRoleRepository roleRepository,
                                 ILogger<RegistrateUseCase> logger,
                                 IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _logger = logger;
            _refreshTokenRepository= refreshTokenRepository;    
        }

        public async Task<RegistrationResponse> ExecuteAsync(RegistrationRequest request, CancellationToken ct)
        {
            try
            {
                if (await _userRepository.GetUserByName(request.Login, ct) != null)
                {
                    return new RegistrationErrorResponse("User already exists", "USER_EXISTS");
                }
                var defaultRole = await _roleRepository.GetRoleByName("User", ct);
                if (defaultRole == null)
                {
                    _logger.LogError("Default 'User' role not found in database");
                    return new RegistrationErrorResponse("Configuration error", "NO_DEFAULT_ROLE");
                }
                var user = new User
                {
                    Login = request.Login,
                    Email = request.Email,
                    PassHash = _passwordService.Hash(request.Password),
                    Roles=new List<Role> { defaultRole }
                };

                await _userRepository.CreateUser(user);
                var accessToken = _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken(user);
                var jti = _tokenService.GetJtiFromToken(refreshToken);
                var expiresAt = _tokenService.GetExpirationFromToken(refreshToken);

                if (string.IsNullOrEmpty(jti))
                {
                    _logger.LogError("Failed to extract JTI from refresh token for new user {Login}", request.Login);
                    return new RegistrationErrorResponse("Token generation failed", "TOKEN_ERROR");
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
                await _refreshTokenRepository.Create(refreshTokenToDb, ct);
                await _unitOfWork.SaveChangesAsync(ct);

                return new RegistrationSuccessResponse(accessToken, refreshToken);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new RegistrationErrorResponse("Unable to registrate at this time", "INVALID_REGISTRATION");
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
