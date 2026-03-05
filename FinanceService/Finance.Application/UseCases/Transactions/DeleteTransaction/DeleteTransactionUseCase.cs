
using Finance.Application.Services;
using Finance.Application.UseCases.Transactions.DeleteTransaction.Request;
using Finance.Application.UseCases.Transactions.DeleteTransaction.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Finance.Application.UseCases.Transactions.DeleteTransaction
{
    public class DeleteTransactionUseCase : IUseCase<DeleteTransactionRequest, DeleteTransactionResponse>
    {
        private readonly ITransactionRepository _transaction;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteTransactionUseCase> _logger;
        private readonly ITransactionCacheInvalidator _cache;
        public DeleteTransactionUseCase(ITransactionRepository transaction,
                                        IUnitOfWork unitOfWork,
                                        ILogger<DeleteTransactionUseCase> logger,
                                        ITransactionCacheInvalidator cache)
        {
            _transaction = transaction;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }
        public async Task<DeleteTransactionResponse> ExecuteAsync(DeleteTransactionRequest request, CancellationToken ct)
        {
            try
            {
                var Transaction = await _transaction.GetTransactionByTransactionId(request.TransactionId,ct);
                await _transaction.DeleteTransaction(request.TransactionId);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(1, request.TransactionId, ct);
                return new DeleteTransactionSuccessRepsonse(request.TransactionId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new DeleteTransactionErrorResponse("Unable to delete Transaction at this time", "INVALID_DELETE");
            }
        }
    }
}
