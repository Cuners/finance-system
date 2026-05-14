using Finance.Application.Common;

namespace Finance.Application.UseCases.Accounts.GetAccountById
{
    public interface IGetAccountByIdUseCase
    {
        Task<Result<GetAccountByIdResult>> ExecuteAsync(
            GetAccountByIdQuery query,
            int userId,
            CancellationToken ct);
    }
}
