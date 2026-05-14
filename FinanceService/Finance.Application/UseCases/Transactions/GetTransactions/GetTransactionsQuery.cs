namespace Finance.Application.UseCases.Transactions.GetTransactions
{
    public record GetTransactionsQuery(
        int? AccountId,
        string? Type = null,
        string? SortOrder = null,
        string? SortBy = null,
        DateOnly? StartDate = null,
        DateOnly? EndDate = null);
}
