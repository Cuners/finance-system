using Finance.Application.Services;
using Finance.Application.UseCases.Accounts.DeleteAccount.Request;
using Finance.Application.UseCases.Accounts.DeleteAccount.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.DeleteAccount
{
    
    public class DeleteAccountUseCase : IUseCase<DeleteAccountRequest, DeleteAccountResponse>
    {
        private readonly IAccountRepository _account;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteAccountUseCase> _logger;
        private readonly IAccountCacheInvalidator _cache;
        public DeleteAccountUseCase(IAccountRepository account, 
                                    IUnitOfWork unitOfWork, 
                                    ILogger<DeleteAccountUseCase> logger,
                                    IAccountCacheInvalidator cache)
        {
            _account = account;
            _unitOfWork = unitOfWork;
            _logger= logger;
            _cache = cache;
        }
        public async Task<DeleteAccountResponse> ExecuteAsync(DeleteAccountRequest request, int userId, CancellationToken ct)
        {
            try
            {
                var account = await _account.GetAccountByAccountId(request.AccountId, ct);
                if (account == null || account.UserId != userId)
                {
                    return new DeleteAccountErrorResponse("Account not found or access denied", "ACCOUNT_ACCESS_DENIED");
                }
                
                await _account.DeleteAccount(request.AccountId);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, request.AccountId, ct);
                return new DeleteAccountSuccessResponse(request.AccountId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new DeleteAccountErrorResponse("Unable to delete account at this time", "INVALID_DELETE");
            }
        }
    }
}
