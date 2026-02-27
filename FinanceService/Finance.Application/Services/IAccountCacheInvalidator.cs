using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Services
{
    public interface IAccountCacheInvalidator
    {
        Task InvalidateAsync(int userId, int accountId, CancellationToken ct = default);
    }
}
