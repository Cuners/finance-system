
using Finance.Application.UseCases.Accounts.GetAccountsByUserId.Response;
using Finance.Application.UseCases.Accounts.GetValueAccounts.Request;
using Finance.Application.UseCases.Accounts.GetValueAccounts.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetValueAccounts
{
    public class GetValueAccountsUseCase
    {
        private readonly IAccountRepository _accounts;
        private readonly ILogger<GetValueAccountsUseCase> _logger;
        public GetValueAccountsUseCase(IAccountRepository accounts,
                                          ILogger<GetValueAccountsUseCase> logger)
        {
            _accounts = accounts;
            _logger = logger;
        }

        public async Task<GetValueAccountsResponse> ExecuteAsync(GetValueAccountsRequest request, CancellationToken ct)
        {
            if (request.UserId <= 0)
            {
                _logger.LogWarning("GetAccountsValue is invalid");
                return new GetValueAccountsErrorResponse("Invalid user id", "INVALID_USER_ID");
            }

            var accounts = await _accounts.GetTotalValue(request.UserId, ct);
            return new GetValueAccountsSuccessResponse(accounts);
        }
    }
}
