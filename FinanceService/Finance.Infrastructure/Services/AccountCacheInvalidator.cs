using Finance.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Infrastructure.Services
{
    public class AccountCacheInvalidator : IAccountCacheInvalidator
    {
        private readonly ICacheService _cache;
        private readonly ILogger<AccountCacheInvalidator> _logger;

        public AccountCacheInvalidator(
            ICacheService cache,
            ILogger<AccountCacheInvalidator> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task InvalidateAsync(int userId, int accountId, CancellationToken ct = default)
        {
            try
            {
                _logger.LogDebug($"Invalidating cache for user {userId}, account {accountId}");
                await Task.WhenAll(
                    _cache.RemoveAsync($"accounts:{accountId}", ct),
                    _cache.RemoveAsync($"accounts:user:{userId}", ct),
                    _cache.RemoveByPatternAsync($"transactions:user:{userId}:", ct),
                    _cache.RemoveByPatternAsync($"dashboard:user:{userId}:", ct),
                    _cache.RemoveByPatternAsync($"budgets:user:{userId}:", ct)
                );
                _logger.LogInformation($"Cache invalidated for user {userId}, transaction {accountId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to invalidate cache for user {userId}, transaction {accountId}");
            }
        }
    }
}
