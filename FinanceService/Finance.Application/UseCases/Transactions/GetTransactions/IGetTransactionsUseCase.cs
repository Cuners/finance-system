using Finance.Application.Common;

namespace Finance.Application.UseCases.Transactions.GetTransactions
{
    public interface IGetTransactionsUseCase
    {
        Task<Result<GetTransactionsResult>> ExecuteAsync(
            GetTransactionsQuery query,
            int userId,
            CancellationToken ct);
    }
}
