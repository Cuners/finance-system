using Finance.Application.Common;

namespace Finance.Application.UseCases.Transactions.DeleteTransaction
{
    public interface IDeleteTransactionUseCase
    {
        Task<Result<DeleteTransactionResult>> ExecuteAsync(
            DeleteTransactionCommand command,
            int userId,
            CancellationToken ct);
    }
}
