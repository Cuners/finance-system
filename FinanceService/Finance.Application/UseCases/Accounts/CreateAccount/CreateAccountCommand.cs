namespace Finance.Application.UseCases.Accounts.CreateAccount
{
    public record CreateAccountCommand(
        string Name,
        decimal Balance,
        string? Note);
}
