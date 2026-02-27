using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Services
{
    public interface IBudgetCacheInvalidator
    {
        Task InvalidateAsync(int userId, int accountId, CancellationToken ct = default);
    }
}
