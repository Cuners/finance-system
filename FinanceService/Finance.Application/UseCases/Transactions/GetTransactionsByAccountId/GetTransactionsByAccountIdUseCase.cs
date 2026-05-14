using Finance.Application.Common;
using Finance.Application.DTO;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Transactions.GetTransactionsByAccountId
{
    public class GetTransactionsByAccountIdUseCase : IGetTransactionsByAccountIdUseCase
    {
        private readonly ITransactionRepository _transactions;
        private readonly ILogger<GetTransactionsByAccountIdUseCase> _logger;

        public GetTransactionsByAccountIdUseCase(
            ITransactionRepository transactions,
            ILogger<GetTransactionsByAccountIdUseCase> logger)
        {
            _transactions = transactions;
            _logger = logger;
        }

        public async Task<Result<GetTransactionsByAccountIdResult>> ExecuteAsync(
            GetTransactionsByAccountIdQuery query,
            int userId,
            CancellationToken ct)
        {
            try
            {
                var transactions = await _transactions.GetTransactionsByAccountId(query.AccountId, ct);
                var result = transactions.Select(ToDto).ToList();
                return Result<GetTransactionsByAccountIdResult>.Success(
                    new GetTransactionsByAccountIdResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get transactions for account {AccountId} and user {UserId}", query.AccountId, userId);
                return Result<GetTransactionsByAccountIdResult>.Failure(
                    "TRANSACTIONS_BY_ACCOUNT_GET_FAILED",
                    "Unable to get transactions at this time");
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
