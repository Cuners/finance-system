using Finance.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Infrastructure.Services
{
    public class TransactionCacheInvalidator : ITransactionCacheInvalidator
    {
        private readonly ICacheService _cache;
        private readonly ILogger<TransactionCacheInvalidator> _logger;

        public TransactionCacheInvalidator(
            ICacheService cache,
            ILogger<TransactionCacheInvalidator> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task InvalidateAsync(int userId, int transactionId, CancellationToken ct = default)
        {
            try
            {
                _logger.LogDebug($"Invalidating cache for user {userId}");
                await Task.WhenAll(
                    _cache.RemoveAsync($"transaction:{transactionId}", ct),
                    _cache.RemoveByPatternAsync($"transactions:user:{userId}:", ct),
                    _cache.RemoveByPatternAsync($"dashboard:user:{userId}:", ct),
                    _cache.RemoveByPatternAsync($"budgets:user:{userId}:", ct)
                );
                _logger.LogInformation("Cache invalidated for user {UserId}, transaction {TransactionId}",
                    userId, transactionId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to invalidate cache for user {UserId}, transaction {TransactionId}",userId, transactionId);
            }
        }

    }
}
