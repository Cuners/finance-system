using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendTransactionNotificationAsync(int userId, decimal amount, string category, CancellationToken ct = default);
        Task SendBudgetExceededNotificationAsync(int userId, string categoryName, decimal percentSpent, CancellationToken ct = default);
        Task SendWelcomeEmailAsync(int userId, string fullName, CancellationToken ct = default);
    }
}
