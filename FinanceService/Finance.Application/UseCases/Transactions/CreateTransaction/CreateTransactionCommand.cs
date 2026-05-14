namespace Finance.Application.UseCases.Transactions.CreateTransaction
{
    public record CreateTransactionCommand(
        int AccountId,
        int CategoryId,
        decimal Amount,
        DateOnly Date,
        string? Note);
}
