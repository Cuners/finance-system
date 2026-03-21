using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Application.Interfaces
{
    public interface IWebSocketSender
    {
        Task SendToUserAsync(int userId, object data, CancellationToken ct = default);
        Task SendToAllAsync(object data, CancellationToken ct = default);
    }
}
