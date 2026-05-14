namespace Finance.Application.UseCases.Transactions.UpdateTransaction
{
    public record UpdateTransactionCommand(
        int TransactionId,
        int AccountId,
        int CategoryId,
        decimal Amount,
        DateOnly Date,
        string? Note);
}
