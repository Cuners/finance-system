using Finance.Application.DTO;

namespace Finance.Application.UseCases.Transactions.GetTransactionsSummary
{
    public record GetTransactionsSummaryResult(TransactionSummaryDto Transactions);
}
