using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain.Interfaces
{
    public interface IBudgetRepository
    {
        Task<Budget?> GetBudgetById(int id, CancellationToken ct);
        Task<IEnumerable<Budget>> GetBudgetsByUserId(int userId, CancellationToken ct);
        Task<IEnumerable<BudgetStatus>> GetBudgetStatusAsync(int userId, CancellationToken ct);
        Task CreateBudget(Budget Budget);
        Task UpdateBudget(Budget Budget);
        Task DeleteBudget(int id);
    }
}
