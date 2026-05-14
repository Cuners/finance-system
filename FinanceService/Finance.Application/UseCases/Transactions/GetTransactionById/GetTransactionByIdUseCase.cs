using Finance.Application.Common;
using Finance.Application.DTO;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Transactions.GetTransactionById
{
    public class GetTransactionByIdUseCase : IGetTransactionByIdUseCase
    {
        private readonly ITransactionRepository _transactions;
        private readonly ILogger<GetTransactionByIdUseCase> _logger;

        public GetTransactionByIdUseCase(
            ITransactionRepository transactions,
            ILogger<GetTransactionByIdUseCase> logger)
        {
            _transactions = transactions;
            _logger = logger;
        }

        public async Task<Result<GetTransactionByIdResult>> ExecuteAsync(
            GetTransactionByIdQuery query,
            int userId,
            CancellationToken ct)
        {
            if (query.TransactionId <= 0)
            {
                return Result<GetTransactionByIdResult>.Failure(
                    "INVALID_TRANSACTION_ID",
                    "Invalid transaction id");
            }

            try
            {
                var transaction = await _transactions.GetTransactionByTransactionId(query.TransactionId, ct);
                if (transaction == null)
                {
                    return Result<GetTransactionByIdResult>.Failure(
                        "TRANSACTION_NOT_FOUND",
                        "No transaction found");
                }

                var dto = ToDto(transaction);
                return Result<GetTransactionByIdResult>.Success(new GetTransactionByIdResult(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get transaction {TransactionId} for user {UserId}", query.TransactionId, userId);
                return Result<GetTransactionByIdResult>.Failure(
                    "TRANSACTION_GET_FAILED",
                    "Unable to get transaction at this time");
            }
        }

        private static TransactionDto ToDto(Domain.Transaction transaction)
        {
            return new TransactionDto(
                transaction.TransactionId,
                transaction.AccountId,
                transaction.Account.Name,
                transaction.CategoryId,
                transaction.Category.Name,
                transaction.Amount,
                transaction.Date,
                transaction.Note);
        }
    }
}
