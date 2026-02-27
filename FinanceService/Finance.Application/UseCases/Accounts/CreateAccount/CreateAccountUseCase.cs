using Finance.Application.UseCases.Accounts.CreateAccount.Request;
using Finance.Application.UseCases.Accounts.CreateAccount.Response;
using Finance.Application.UseCases.Accounts.UpdateAccount;
using Finance.Domain.Interfaces;
using Microsoft.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
namespace Finance.Application.UseCases.Accounts.CreateAccount
{
    public class CreateAccountUseCase
    {
        private readonly IAccountRepository _accounts;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateAccountUseCase> _logger;
       
        public CreateAccountUseCase(IAccountRepository accounts, 
                                    IUnitOfWork unitOfWork,
                                    ILogger<CreateAccountUseCase> logger)
        {
            _accounts = accounts;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CreateAccountResponse> ExecuteAsync(CreateAccountRequest request, CancellationToken ct)
        {
            try
            {
                var account = new Domain.Account
                {
                    UserId = request.UserId,
                    Name = request.Name,
                    Balance = request.Balance
                };
                await _accounts.CreateAccount(account);
                await _unitOfWork.SaveChangesAsync(ct);
                return new CreateAccountSuccessResponse(account.AccountId);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new CreateAccountErrorResponse("Unable to create account at this time", "INVALID_CREATE");
            }
        }
    }
}
