using Finance.Domain;
using Finance.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Infrastructure.Persistence.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly BudgetDbContext _context;
        public BudgetRepository(BudgetDbContext context)
        {
            _context = context;
        }
        public async Task<Budget?> GetBudgetById(int id, CancellationToken ct)
        {
            return await _context.Budgets
                .AsNoTracking()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.BudgetId == id,ct);
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByUserId(int userId, CancellationToken ct)
        {
            return await _context.Budgets
                .AsNoTracking()
                .Include(x=>x.Category)
                .Where(x=>x.UserId == userId)
                .ToListAsync(ct);
        }
        public async Task<IEnumerable<BudgetStatus>> GetBudgetStatusAsync(int userId, CancellationToken ct)
        {
            return await _context.Budgets
                .Where(b => b.UserId == userId)
                .Select(b => new
                {
                    Budget = b,
                    Spent = _context.Transactions
                        .Where(t => t.Account.UserId == userId
                                 && t.CategoryId == b.CategoryId
                                 && t.Amount < 0
                                 && t.Date.Year == b.Date.Year
                                 && t.Date.Month == b.Date.Month
                                 )
                        .Sum(t => -t.Amount),
                })
                .Select(x => new BudgetStatus(
                    BudgetId:x.Budget.BudgetId,
                    CategoryName:x.Budget.Category.Name,
                    LimitAmount:x.Budget.LimitAmount,
                    TotalSpent:x.Spent,
                    ProcentSpent:Math.Round(x.Spent/(x.Budget.LimitAmount/100)),
                    Remaining:x.Budget.LimitAmount - x.Spent,
                    Period:new DateOnly(x.Budget.Date.Year, x.Budget.Date.Month, 1)
                ))
                .ToListAsync(ct);
        }

        public async Task CreateBudget(Budget Budget)
        {
            await _context.Budgets.AddAsync(Budget);
        }

        public async Task UpdateBudget(Budget Budget)
        {
            _context.Budgets.Update(Budget);
        }

        public async Task DeleteBudget(int id)
        {
            var acc = await _context.Budgets.FindAsync(id);
            if (acc != null)
                _context.Budgets.Remove(acc);
        }
    }
}
