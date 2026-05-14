using Finance.Application.DTO;

namespace Finance.Application.UseCases.Transactions.GetTransactionsByAccountId
{
    public record GetTransactionsByAccountIdResult(IReadOnlyList<TransactionDto> Transactions);
}
