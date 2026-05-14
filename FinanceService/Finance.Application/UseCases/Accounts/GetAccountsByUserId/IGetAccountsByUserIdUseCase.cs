using Finance.Application.Common;

namespace Finance.Application.UseCases.Accounts.GetAccountsByUserId
{
    public interface IGetAccountsByUserIdUseCase
    {
        Task<Result<GetAccountsByUserIdResult>> ExecuteAsync(
            GetAccountsByUserIdQuery query,
            int userId,
            CancellationToken ct);
    }
}
