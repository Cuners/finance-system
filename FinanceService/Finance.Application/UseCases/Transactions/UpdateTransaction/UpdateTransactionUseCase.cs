using Finance.Application.Services;
using Finance.Application.UseCases.Accounts.UpdateAccount;
using Finance.Application.UseCases.Accounts.UpdateAccount.Request;
using Finance.Application.UseCases.Accounts.UpdateAccount.Response;
using Finance.Application.UseCases.Transactions.UpdateTransaction.Request;
using Finance.Application.UseCases.Transactions.UpdateTransaction.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.UpdateTransaction
{
    public class UpdateTransactionUseCase : IUseCase<UpdateTransactionRequest,UpdateTransactionResponse>
    {
        private readonly ITransactionRepository _transaction;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateTransactionUseCase> _logger;
        private readonly IAccountCacheInvalidator _cache;
        public UpdateTransactionUseCase(ITransactionRepository transaction,
                                    IUnitOfWork unitOfWork,
                                    ILogger<UpdateTransactionUseCase> logger,
                                    IAccountCacheInvalidator cache)
        {
            _transaction = transaction;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }
        public async Task<UpdateTransactionResponse> ExecuteAsync(UpdateTransactionRequest request,int userId, CancellationToken ct)
        {
            try
            {
                var transaction = await _transaction.GetTransactionByTransactionId(request.TransactionId, ct);
                transaction.AccountId= request.AccountId;
                transaction.CategoryId = request.CategoryId;
                transaction.Amount= request.Amount;
                transaction.Note = request.Note;
                await _transaction.UpdateTransaction(transaction);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, request.AccountId, ct);
                return new UpdateTransactionSuccessResponse(transaction.TransactionId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new UpdateTransactionErrorResponse("Unable to update account at this time", "INVALID_UPDATE");
            }
        }
    }
}
