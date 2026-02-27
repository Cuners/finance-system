using Finance.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetTransactionByTransactionId(int id, CancellationToken ct);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountId(int id, CancellationToken ct);
        Task<IEnumerable<Transaction>> GetTransactions(TransactionFilter transactionFilter,CancellationToken ct);
        Task<FinancialSummary> GetTransactionsSummaryAsync(int userId, int year, int month, CancellationToken ct);
        Task CreateTransaction(Transaction Transaction);
        Task UpdateTransaction(Transaction Transaction);
        Task DeleteTransaction(int id);
    }
}
