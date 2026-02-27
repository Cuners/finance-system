using Finance.Application.UseCases.Accounts.GetAccountsByUserId.Request;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetAccountsByUserId
{
    public class GetAccountsByUserIdUseCase
    {
        private readonly IAccountRepository _accounts;
        private readonly ILogger<GetAccountsByUserIdUseCase> _logger;
        public GetAccountsByUserIdUseCase(IAccountRepository accounts, 
                                          ILogger<GetAccountsByUserIdUseCase> logger)
        {
            _accounts = accounts;
            _logger=logger;
        }

        public async Task<GetAccountsByUserIdResponse> ExecuteAsync(GetAccountsByUserIdRequest request, CancellationToken ct)
        {
            if (request.UserId <= 0)
            {
                _logger.LogWarning("GetAccountsByUserIdRequest is invalid");
                return new GetAccountsByUserIdErrorResponse("Invalid user id", "INVALID_USER_ID");
            }

            var accounts = await _accounts.GetAccountsByUserId(request.UserId, ct);
            if (accounts == null || !accounts.Any())
            {
                _logger.LogWarning("GetAccountsByUserIdRequest account is null");
                return new GetAccountsByUserIdErrorResponse("No accounts found", "ACCOUNTS_NOT_FOUND");
            }

            var result = accounts.Select(a => new AccountDto
            {
                AccountId = a.AccountId,
                Name = a.Name,
                Balance = a.Balance
            }).ToList();

            return new GetAccountsByUserIdSuccessResponse(result);
        }
    }
}
