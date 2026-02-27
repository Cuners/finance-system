using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
        Task<T?> GetOrCreateAsync<T>(string key,
                                     Func<CancellationToken, Task<T>> factory,
                                     DistributedCacheEntryOptions? options = null,
                                     TimeSpan? expiration = null,
                                     CancellationToken ct = default);
        Task RemoveAsync(string key, CancellationToken ct = default);
        Task RemoveByPatternAsync(string pattern, CancellationToken ct = default);
    }
}
