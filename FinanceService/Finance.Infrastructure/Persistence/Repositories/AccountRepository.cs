using Finance.Domain;
using Finance.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace Finance.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BudgetDbContext _context;
        public AccountRepository(BudgetDbContext context)
        {
            _context = context;
        }
        public async Task<Account?> GetAccountByAccountId(int id, CancellationToken ct)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(x => x.AccountId == id, ct);
        }
        public async Task<decimal> GetTotalValue(int userid,CancellationToken ct)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(x => x.UserId == userid)
                .SumAsync(x=>x.Balance,ct);
        }

        public async Task<IEnumerable<Account>> GetAllAccounts(CancellationToken ct)
        {
            return await _context.Accounts
                .AsNoTracking()
                .ToListAsync(ct);
        }
        public async Task<IEnumerable<Account>> GetAccountsByUserId(int userId, CancellationToken ct)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync(ct);
        }
        public async Task CreateAccount(Account account)
        {
            await _context.Accounts.AddAsync(account);
        }

        public async Task UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
        }

        public async Task DeleteAccount(int id)
        {
            var acc = await _context.Accounts.FindAsync(id);
            if (acc != null)
            {
                _context.Accounts.Remove(acc);
            }
        }
    }
}
