using Finance.Application.UseCases.Accounts.DeleteAccount.Response;
using Finance.Application.UseCases.Accounts.UpdateAccount.Request;
using Finance.Application.UseCases.Accounts.UpdateAccount.Response;
using Finance.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Finance.Application.Services;
namespace Finance.Application.UseCases.Accounts.UpdateAccount
{
    public class UpdateAccountUseCase
    {
        private readonly IAccountRepository _account;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateAccountUseCase> _logger;
        private readonly IAccountCacheInvalidator _cache;
        public UpdateAccountUseCase(IAccountRepository account,
                                    IUnitOfWork unitOfWork,
                                    ILogger<UpdateAccountUseCase> logger,
                                    IAccountCacheInvalidator cache)
        {
            _account = account;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }
        public async Task<UpdateAccountResponse> ExecuteAsync(UpdateAccountRequest request, CancellationToken ct)
        {
            try
            {
                var account = await _account.GetAccountByAccountId(request.AccountId,ct);
                account.Balance = request.Balance;
                account.Name = request.Name;
                await _account.UpdateAccount(account);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(1, request.AccountId, ct);
                return new UpdateAccountSuccessResponse(account.AccountId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new UpdateAccountErrorResponse("Unable to update account at this time", "INVALID_UPDATE");
            }
        }
    }
}
