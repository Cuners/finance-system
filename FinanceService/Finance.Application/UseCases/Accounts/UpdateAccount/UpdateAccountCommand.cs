namespace Finance.Application.UseCases.Accounts.UpdateAccount
{
    public record UpdateAccountCommand(
        int AccountId,
        string Name,
        decimal Balance,
        string? Note);
}
