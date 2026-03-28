using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Services
{
    public interface IEventPublisher
    {
        Task PublishBudgetExceededAsync(int userId, string accountName, string email, decimal balance, decimal spentAmount, int transactionId, CancellationToken ct = default);
    }
}
