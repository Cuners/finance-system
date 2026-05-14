using Finance.Application.Common;

namespace Finance.Application.UseCases.Accounts.CreateAccount
{
    public interface ICreateAccountUseCase
    {
        Task<Result<CreateAccountResult>> ExecuteAsync(
            CreateAccountCommand command,
            int userId,
            CancellationToken ct);
    }
}
