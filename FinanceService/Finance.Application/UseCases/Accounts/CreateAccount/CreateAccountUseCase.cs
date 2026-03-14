using Finance.Application.Services;
using Finance.Application.UseCases.Accounts.CreateAccount.Request;
using Finance.Application.UseCases.Accounts.CreateAccount.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
namespace Finance.Application.UseCases.Accounts.CreateAccount
{
    public class CreateAccountUseCase : IUseCase<CreateAccountRequest, CreateAccountResponse>
    {
        private readonly IAccountRepository _accounts;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateAccountUseCase> _logger;
        private readonly IAccountCacheInvalidator _cache;
        public CreateAccountUseCase(IAccountRepository accounts, 
                                    IUnitOfWork unitOfWork,
                                    ILogger<CreateAccountUseCase> logger,
                                    IAccountCacheInvalidator cache)
        {
            _accounts = accounts;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<CreateAccountResponse> ExecuteAsync(CreateAccountRequest request, int userId, CancellationToken ct)
         {
             try
             {
                 var account = new Domain.Account
                 {
                     UserId = userId,
                     Name = request.Name,
                     Balance = request.Balance,
                     Note= request.Note
                 };
                 await _accounts.CreateAccount(account);
                 await _unitOfWork.SaveChangesAsync(ct);
                 await _cache.InvalidateAsync(userId,account.AccountId, ct);
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
