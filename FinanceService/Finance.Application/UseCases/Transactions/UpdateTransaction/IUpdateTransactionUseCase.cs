using Finance.Application.Common;

namespace Finance.Application.UseCases.Transactions.UpdateTransaction
{
    public interface IUpdateTransactionUseCase
    {
        Task<Result<UpdateTransactionResult>> ExecuteAsync(
            UpdateTransactionCommand command,
            int userId,
            CancellationToken ct);
    }
}
