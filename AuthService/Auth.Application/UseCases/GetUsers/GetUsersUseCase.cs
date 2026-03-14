using Auth.Application.UseCases.GetUsers.Request;
using Auth.Application.UseCases.GetUsers.Response;
using Auth.Domain;
using Auth.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.GetUsers
{
    public class GetUsersUseCase : IUseCase<UsersRequest, UsersResponse>
    {
        private readonly IUserRepository _Users;
        private readonly ILogger<GetUsersUseCase> _logger;
        public GetUsersUseCase(IUserRepository users, ILogger<GetUsersUseCase> logger)
        {
            _Users = users;
            _logger = logger;
        }
        public async Task<UsersResponse> ExecuteAsync(UsersRequest request,CancellationToken ct)
        {
            try
            {
                var users = await _Users.GetAllUsers(ct);
                if (!users.Any() || users == null)
                {
                    _logger.LogError("users is null");
                    return new UsersErrorResponse("Invalid users", "Invalid user");
                }
                var results = users;
                var result = users.Select(x => new User
                {
                    UserId=x.UserId,
                    Login=x.Login,
                    PassHash=x.PassHash,
                    Email=x.Email
                });
                return new UsersSuccessResponse(results);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new UsersErrorResponse("Unable to get users at this time", "INVALID_Users");
            }
        }

    }
}
