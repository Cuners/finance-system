using Finance.Application.Common;

namespace Finance.Application.UseCases.Transactions.GetTransactionsSummary
{
    public interface IGetTransactionsSummaryUseCase
    {
        Task<Result<GetTransactionsSummaryResult>> ExecuteAsync(
            GetTransactionsSummaryQuery query,
            int userId,
            CancellationToken ct);
    }
}
