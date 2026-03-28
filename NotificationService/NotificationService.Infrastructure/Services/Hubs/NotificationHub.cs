using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace NotificationService.Infrastructure.Services.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {

        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }
        public async Task Subscribe()
        {
            var currentUserId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                _logger.LogWarning($"Invalid or missing UserId claim for connection {currentUserId}");
                throw new HubException("Unauthorized");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{currentUserId}");
            _logger.LogInformation($"User {currentUserId} subscribed to notifications from {Context.ConnectionId}");
        }

        public async Task Unsubscribe()
        {
            var currentUserIdClaim = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(currentUserIdClaim, out var currentUserId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{currentUserId}");
                _logger.LogInformation($"User {currentUserId} unsubscribed from notifications");
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
