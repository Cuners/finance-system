using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Services
{
    public interface ITransactionCacheInvalidator
    {
        Task InvalidateAsync(int userId, int transactionId, CancellationToken ct = default);
    }
}
