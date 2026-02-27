using Auth.Application.Services;
using Auth.Application.UseCases.LoginUser.Request;
using Auth.Application.UseCases.LoginUser.Response;
using Auth.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.LoginUser
{
    public class LoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginUseCase> _logger;
        public LoginUseCase(IUserRepository userRepository, IPasswordService passwordService, ITokenService tokenService, ILogger<LoginUseCase> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LoginRepsonse> ExecuteAsync(LoginRequest request, CancellationToken ct)
        {
            try
            {

                var user = await _userRepository.GetUserByName(request.Username, ct);
                //if (user == null || !_passwordService.Verify(user.PassHash, request.Password))
                //{
                //    return new LoginErrorResponse("Invalid credentials", "INVALID_CREDENTIALS");
                //}
                if (user == null)
                {
                    return new LoginErrorResponse("Invalid Login", "INVALID_Login"); ;
                }
                var claims = _tokenService.GenerateUserClaims(user);
                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken(claims);

                return new LoginSuccessRepsonse(accessToken, refreshToken);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new LoginErrorResponse("Unable to login at this time", "INVALID_LOGIN");
            }
        }
    }
}
