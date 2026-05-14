using Finance.Application.Common;

namespace Finance.Application.UseCases.Accounts.GetValueAccounts
{
    public interface IGetValueAccountsUseCase
    {
        Task<Result<GetValueAccountsResult>> ExecuteAsync(
            GetValueAccountsQuery query,
            int userId,
            CancellationToken ct);
    }
}
