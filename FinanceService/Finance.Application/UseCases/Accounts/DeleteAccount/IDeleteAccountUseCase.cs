using Finance.Application.Common;

namespace Finance.Application.UseCases.Accounts.DeleteAccount
{
    public interface IDeleteAccountUseCase
    {
        Task<Result<DeleteAccountResult>> ExecuteAsync(
            DeleteAccountCommand command,
            int userId,
            CancellationToken ct);
    }
}
