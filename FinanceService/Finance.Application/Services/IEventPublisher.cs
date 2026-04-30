using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Services
{
    public interface IEventPublisher
    {
        Task PublishTransactionCreatedAsync(int userId,  string email, string accountName, decimal balance, decimal spentAmount, int transactionId, CancellationToken ct = default);
    }
}
