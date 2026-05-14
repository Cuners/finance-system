using Finance.Application.Common;

namespace Finance.Application.UseCases.Transactions.GetTransactionsByAccountId
{
    public interface IGetTransactionsByAccountIdUseCase
    {
        Task<Result<GetTransactionsByAccountIdResult>> ExecuteAsync(
            GetTransactionsByAccountIdQuery query,
            int userId,
            CancellationToken ct);
    }
}
