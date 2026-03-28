using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Application.Interfaces
{
    public interface IUserEmailService
    {
        Task<string?> GetUserEmailAsync(int userId, CancellationToken ct = default);
    }
}
