using Finance.Application.UseCases.Accounts.DeleteAccount.Request;
using Finance.Application.UseCases.Accounts.DeleteAccount.Response;
using Finance.Domain;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Accounts.DeleteAccount
{
    
    public class DeleteAccountUseCase
    {
        private readonly IAccountRepository _account;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteAccountUseCase> _logger;
        public DeleteAccountUseCase(IAccountRepository account, 
                                    IUnitOfWork unitOfWork, 
                                    ILogger<DeleteAccountUseCase> logger)
        {
            _account = account;
            _unitOfWork = unitOfWork;
            _logger= logger;
        }
        public async Task<DeleteAccountResponse> ExecuteAsync(DeleteAccountRequest request, CancellationToken ct) 
        {
            try
            {
                var account = await _account.GetAccountByAccountId(request.AccountId, ct);
                await _account.DeleteAccount(request.AccountId);
                await _unitOfWork.SaveChangesAsync(ct);
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
