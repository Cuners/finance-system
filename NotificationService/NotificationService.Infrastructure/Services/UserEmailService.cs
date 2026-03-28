using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using NotificationService.Application.Interfaces;
namespace NotificationService.Infrastructure.Services
{
    public class UserEmailService : IUserEmailService
    {

        private readonly ILogger<UserEmailService> _logger;

        public UserEmailService(ILogger<UserEmailService> logger)
        {
            _logger = logger;
        }
        public Task<string?> GetUserEmailAsync(int userId, CancellationToken ct = default)
        {
            _logger.LogWarning($"GetUserEmailAsync called for user {userId} — email should be in event");
            return Task.FromResult<string?>(null);
        }
    }
}
