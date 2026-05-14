using Finance.Application.Common;

namespace Finance.Application.UseCases.Accounts.UpdateAccount
{
    public interface IUpdateAccountUseCase
    {
        Task<Result<UpdateAccountResult>> ExecuteAsync(
            UpdateAccountCommand command,
            int userId,
            CancellationToken ct);
    }
}
