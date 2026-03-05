using Azure.Core;
using Finance.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Infrastructure.Services
{
    public class BudgetCacheInvalidator : IBudgetCacheInvalidator
    {
        private readonly ICacheService _cache;
        private readonly ILogger<BudgetCacheInvalidator> _logger;

        public BudgetCacheInvalidator(
            ICacheService cache,
            ILogger<BudgetCacheInvalidator> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task InvalidateAsync(int userId, int budgetId, CancellationToken ct = default)
        {
            try
            {
                _logger.LogDebug($"Invalidating cache for user {userId}, budget {budgetId}");

                await Task.WhenAll(
                    _cache.RemoveByPatternAsync($"dashboard:user:{userId}:", ct),
                    _cache.RemoveByPatternAsync($"budgets:user:{userId}:", ct)
                );
                _logger.LogInformation("Cache invalidated for user {UserId}, transaction {TransactionId}",
                    userId, budgetId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to invalidate cache for user {userId}, budget {budgetId}");
            }
        }
    }
}
