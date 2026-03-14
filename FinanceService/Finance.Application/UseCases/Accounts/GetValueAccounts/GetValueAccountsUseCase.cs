
using Finance.Application.Services;
using Finance.Application.UseCases.Accounts.GetValueAccounts.Request;
using Finance.Application.UseCases.Accounts.GetValueAccounts.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetValueAccounts
{
    public class GetValueAccountsUseCase : IUseCase<GetValueAccountsRequest, GetValueAccountsResponse>
    {
        private readonly IAccountRepository _accounts;
        private readonly ILogger<GetValueAccountsUseCase> _logger;
        private readonly ICacheService _cache;
        public GetValueAccountsUseCase(IAccountRepository accounts,
                                       ILogger<GetValueAccountsUseCase> logger,
                                       ICacheService cache)
        {
            _accounts = accounts;
            _logger = logger;
            _cache = cache;
        }

        public async Task<GetValueAccountsResponse> ExecuteAsync(GetValueAccountsRequest request, int userId, CancellationToken ct)
        {  
            var cacheKey = $"accounts:user:{userId}:" + "accountValue";
            try
            {
                var accountValue = await _cache.GetOrCreateAsync<decimal?>(cacheKey, async token =>
                {
                    var accounts = await _accounts.GetTotalValue(userId, ct);
                    return accounts;
                });
                return new GetValueAccountsSuccessResponse(accountValue.Value);
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, $"Cache lock timeout for key {cacheKey}");
                var accountValue = await _accounts.GetTotalValue(userId, ct);
                return new GetValueAccountsSuccessResponse(accountValue);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetValueAccountsErrorResponse("Unable to get Balance for Accounts at this time", "INVALID_GET");
            }
        }
    }
}
