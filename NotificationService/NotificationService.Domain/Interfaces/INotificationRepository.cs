using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> SaveAsync(Notification notification, CancellationToken ct = default);
        Task<Notification?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<Notification>> GetByUserIdAsync(int userId, int page, int pageSize, CancellationToken ct = default);
        Task<int> GetUnreadCountAsync(int userId, CancellationToken ct = default);
        Task MarkAsReadAsync(int id, int userId, CancellationToken ct = default);
        Task MarkAllAsReadAsync(int userId, CancellationToken ct = default);
        Task UpdateAsync(Notification notification, CancellationToken ct = default);
    }
}
