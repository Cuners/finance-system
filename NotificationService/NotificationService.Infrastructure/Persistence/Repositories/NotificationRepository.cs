using Microsoft.EntityFrameworkCore;
using NotificationService.Domain;
using NotificationService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> SaveAsync(Notification notification, CancellationToken ct = default)
        {
            await _context.Notifications.AddAsync(notification, ct);
            await _context.SaveChangesAsync(ct);
            return notification;
        }

        public async Task<Notification?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Notifications.FirstOrDefaultAsync(x=>x.NotificationId==id, ct);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, int page, int pageSize, CancellationToken ct = default)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> GetUnreadCountAsync(int userId, CancellationToken ct = default)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead, ct);
        }

        public async Task MarkAsReadAsync(int id, int userId, CancellationToken ct = default)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == id && n.UserId == userId, ct);

            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task MarkAllAsReadAsync(int userId, CancellationToken ct = default)
        {
            await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(setters => setters.SetProperty(n => n.IsRead, true), ct);
        }

        public async Task UpdateAsync(Notification notification, CancellationToken ct = default)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync(ct);
        }
    }
}
