using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace NotificationService.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public async Task Subscribe()
        {
            var currentUserId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                _logger.LogWarning($"Invalid User {currentUserId} tried subscribe to notifications");
                throw new HubException("Unauthorized");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{currentUserId}");
            _logger.LogInformation($"User {currentUserId} subscribed to notifications from {Context.ConnectionId}");
        }

        public async Task Unsubscribe()
        {
            var currentUserId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{currentUserId}");
            _logger.LogInformation($"User {currentUserId} unsubscribed from notifications");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
