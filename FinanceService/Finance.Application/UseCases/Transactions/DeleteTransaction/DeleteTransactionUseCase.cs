using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Transactions.DeleteTransaction
{
    public class DeleteTransactionUseCase : IDeleteTransactionUseCase
    {
        private readonly ITransactionRepository _transactions;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteTransactionUseCase> _logger;
        private readonly ITransactionCacheInvalidator _cache;

        public DeleteTransactionUseCase(
            ITransactionRepository transactions,
            IUnitOfWork unitOfWork,
            ILogger<DeleteTransactionUseCase> logger,
            ITransactionCacheInvalidator cache)
        {
            _transactions = transactions;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<DeleteTransactionResult>> ExecuteAsync(
            DeleteTransactionCommand command,
            int userId,
            CancellationToken ct)
        {
            try
            {
                var transaction = await _transactions.GetTransactionByTransactionId(command.TransactionId, ct);
                if (transaction == null)
                {
                    return Result<DeleteTransactionResult>.Failure(
                        "TRANSACTION_NOT_FOUND",
                        "Transaction not found");
                }

                await _transactions.DeleteTransaction(command.TransactionId);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, command.TransactionId, ct);

                return Result<DeleteTransactionResult>.Success(new DeleteTransactionResult(command.TransactionId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete transaction {TransactionId} for user {UserId}", command.TransactionId, userId);
                return Result<DeleteTransactionResult>.Failure(
                    "TRANSACTION_DELETE_FAILED",
                    "Unable to delete transaction at this time");
            }
        }
    }
}
