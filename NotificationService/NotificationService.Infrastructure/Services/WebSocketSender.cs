using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Services.Hubs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Infrastructure.Services
{
    public class WebSocketSender : IWebSocketSender
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<WebSocketSender> _logger;

        public WebSocketSender(
            IHubContext<NotificationHub> hubContext,
            ILogger<WebSocketSender> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task SendToUserAsync(int userId, object data, CancellationToken ct = default)
        {
            try
            {
                await _hubContext.Clients.Group($"user-{userId}").SendAsync("ReceiveNotification", data, ct);
                _logger.LogDebug("WebSocket notification sent to user {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send WebSocket notification to user {userId}");
            }
        }

        public async Task SendToAllAsync(object data, CancellationToken ct = default)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", data, ct);
                _logger.LogDebug("WebSocket notification sent to all users");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send WebSocket notification to all users");
            }
        }
    }
}
