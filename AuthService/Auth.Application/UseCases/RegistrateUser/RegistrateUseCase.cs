using Auth.Application.Services;
using Auth.Application.UseCases.RegistrateUser.Request;
using Auth.Application.UseCases.RegistrateUser.Response;
using Auth.Domain;
using Auth.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.RegistrateUser
{
    public class RegistrateUseCase : IUseCase<RegistrationRequest,RegistrationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegistrateUseCase> _logger;
        public RegistrateUseCase(IUserRepository userRepository, 
                                 IUnitOfWork unitOfWork, 
                                 IPasswordService passwordService, 
                                 ITokenService tokenService, 
                                 ILogger<RegistrateUseCase> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<RegistrationResponse> ExecuteAsync(RegistrationRequest request, CancellationToken ct)
        {
            try
            {
                if (await _userRepository.GetUserByName(request.Login,ct) != null)
                    return new RegistrationErrorResponse("User already exists", "USER_EXISTS");

                var user = new User
                {
                    Login = request.Login,
                    Email = request.Email,
                    PassHash = _passwordService.Hash(request.Password)
                };

                await _userRepository.CreateUser(user);
                await _unitOfWork.SaveChangesAsync(ct);
                var claims = _tokenService.GenerateUserClaims(user);
                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken(claims);

                return new RegistrationSuccessResponse(accessToken, refreshToken);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new RegistrationErrorResponse("Unable to registrate at this time", "INVALID_REGISTRATION");
            }
        }
    }
}
