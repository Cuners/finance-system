using Finance.Application.DTO;

namespace Finance.Application.UseCases.Transactions.GetTransactions
{
    public record GetTransactionsResult(IReadOnlyList<TransactionDto> Transactions);
}
