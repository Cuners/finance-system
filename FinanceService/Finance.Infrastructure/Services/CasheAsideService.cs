using Finance.Application.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Finance.Infrastructure.Services
{
    public class CacheAsideService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<CacheAsideService> _logger;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks
            = new ConcurrentDictionary<string, SemaphoreSlim>();
        private readonly DistributedCacheEntryOptions _defaultOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        };

        private readonly TimeSpan _defaultLockTimeout = TimeSpan.FromSeconds(30);
        public CacheAsideService(IDistributedCache cache, 
                                 IConnectionMultiplexer redis,
                                 ILogger<CacheAsideService> logger)
        {
            _cache = cache;
            _redis = redis;
            _logger = logger;
        }
        public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            try
            {
                var cachedValue = await _cache.GetStringAsync(key, ct);

                if (string.IsNullOrEmpty(cachedValue))
                {
                    return default;
                }
                try
                {
                    var value = JsonSerializer.Deserialize<T>(cachedValue);
                    if (value is not null)
                    {
                        return value;
                    }
                    return default;
                }
                catch (JsonException)
                {
                    await _cache.RemoveAsync(key, ct);
                    return default;
                }
            }
            catch(RedisConnectionException ex)
            {
                _logger?.LogError(ex, $"Redis connection failed for key '{key}'");
                throw new TimeoutException($"Redis connection error for key '{key}'", ex);
            }
        }
        public async Task<T?> GetOrCreateAsync<T>(string key,
                                                  Func<CancellationToken, Task<T>> factory,
                                                  DistributedCacheEntryOptions? options = null,
                                                  TimeSpan? lockTimeout = null,
                                                  CancellationToken cancellationToken = default)
        {
            var cachedValue = await GetAsync<T>(key, cancellationToken);
            var semaphore = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
            var timeout = lockTimeout ?? _defaultLockTimeout;

            var hasLock = await semaphore.WaitAsync(timeout, cancellationToken);

            if (!hasLock)
            {
                await Task.Delay(100, cancellationToken);

                cachedValue = await GetAsync<T>( key, cancellationToken);
                if (cachedValue!=null)
                {
                    return cachedValue;
                }
                throw new TimeoutException($"Failed to acquire cache lock for key '{key}' within {timeout}");
            }

            try
            {
                cachedValue = await GetAsync<T>(key, cancellationToken);
                if (cachedValue != null)
                {
                    return cachedValue;
                }
                var value = await factory(cancellationToken);
                if (value is null)
                {
                    return default;
                }
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), options ?? _defaultOptions, cancellationToken);

                return value;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task RemoveAsync(string key,
                                      CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key, cancellationToken);
            if (_locks.TryRemove(key, out var semaphore))
            {
                semaphore.Dispose();
            }
        }
        public async Task RemoveByPatternAsync(string pattern, CancellationToken ct = default)
        {
            try
            {
                if (!_redis.IsConnected)
                {
                    _logger?.LogWarning("Redis Multiplexer is not connected. Skipping invalidation.");
                    return;
                }

                var endpoint = _redis.GetEndPoints().First();
                var server = _redis.GetServer(endpoint);
                var keys = server.Keys(pattern: $"{pattern}*", pageSize: 1000).ToArray();

                foreach (var key in keys)
                {
                    var keyStr = key.ToString();
                    await _cache.RemoveAsync(keyStr, ct);

                    if (_locks.TryRemove(keyStr, out var semaphore))
                    {
                        semaphore.Dispose();
                    }
                }
                _logger?.LogInformation($"Invalidated {keys.Count()} cache keys for pattern {pattern}");
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, $"Failed to invalidate cache pattern {pattern}");
            }
        }
    }
}
