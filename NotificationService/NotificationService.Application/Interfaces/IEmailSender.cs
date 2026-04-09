using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendTransactionNotificationAsync(int userId,
                                              string email,
                                              string accountName,
                                              decimal balance,
                                              decimal spentAmount, 
                                              CancellationToken ct = default);
        Task SendBudgetExceededNotificationAsync(int userId, 
                                                 string email, 
                                                 string categoryName, 
                                                 decimal percentSpent, 
                                                 CancellationToken ct = default);
        Task SendWelcomeEmailAsync(int userId, 
                                   string email, 
                                   string fullName, 
                                   CancellationToken ct = default);
    }
}
