using Finance.Application.UseCases.Accounts.GetAccountById.Request;
using Finance.Application.UseCases.Accounts.GetAccountById.Response;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.GetAccountById
{
    public class GetAccountByIdUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<GetAccountByIdUseCase> _logger;
        public GetAccountByIdUseCase(IAccountRepository accountRepository, 
                                     ILogger<GetAccountByIdUseCase> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }
        public async Task<GetAccountByIdResponse> ExecuteAsync(GetAccountByIdRequest request, CancellationToken ct)
        {
            try
            {
                if (request.AccountId <= 0)
                {
                    _logger.LogWarning("GetAccountRequest is invalid");
                    return new GetAccountByIdErrorResponse("Invalid account id", "INVALID_USER_ID");
                }
                var accounts = await _accountRepository.GetAccountByAccountId(request.AccountId,ct);
                if (accounts == null)
                {
                    _logger.LogWarning("GetAccountRequest account is null");
                    return new GetAccountByIdErrorResponse("No account found", "ACCOUNT_NOT_FOUND");
                }
                var result = new AccountDto
                {
                    AccountId = accounts.AccountId,
                    Name = accounts.Name,
                    Balance = accounts.Balance
                };
                return new GetAccountByIdSuccessResponse(result);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetAccountByIdErrorResponse("Unable to get account at this time", "INVALID_GET");
            }
        }
    }
}
