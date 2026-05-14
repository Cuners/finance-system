using Finance.Application.Common;

namespace Finance.Application.UseCases.Transactions.CreateTransaction
{
    public interface ICreateTransactionUseCase
    {
        Task<Result<CreateTransactionResult>> ExecuteAsync(
            CreateTransactionCommand command,
            int userId,
            CancellationToken ct);
    }
}
