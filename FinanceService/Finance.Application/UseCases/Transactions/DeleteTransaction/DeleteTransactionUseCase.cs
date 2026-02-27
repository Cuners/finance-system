
using Finance.Application.UseCases.Transactions.DeleteTransaction.Request;
using Finance.Application.UseCases.Transactions.DeleteTransaction.Response;
using Finance.Domain;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.DeleteTransaction
{
    public class DeleteTransactionUseCase
    {
        private readonly ITransactionRepository _transaction;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteTransactionUseCase> _logger;

        public DeleteTransactionUseCase(ITransactionRepository transaction,
                                        IUnitOfWork unitOfWork,
                                        ILogger<DeleteTransactionUseCase> logger)
        {
            _transaction = transaction;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<DeleteTransactionResponse> ExecuteAsync(DeleteTransactionRequest request, CancellationToken ct)
        {
            try
            {
                var Transaction = await _transaction.GetTransactionByTransactionId(request.TransactionId,ct);
                await _transaction.DeleteTransaction(request.TransactionId);
                await _unitOfWork.SaveChangesAsync(ct);
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
