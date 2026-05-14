using Finance.Application.Common;

namespace Finance.Application.UseCases.Transactions.GetTransactionById
{
    public interface IGetTransactionByIdUseCase
    {
        Task<Result<GetTransactionByIdResult>> ExecuteAsync(
            GetTransactionByIdQuery query,
            int userId,
            CancellationToken ct);
    }
}
