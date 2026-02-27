using Azure.Core;
using Finance.Domain;
using Finance.Domain.Interfaces;
using Finance.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Finance.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BudgetDbContext _context;
        public TransactionRepository(BudgetDbContext context)
        {
            _context = context;
        }
        public async Task<Transaction?> GetTransactionByTransactionId(int id, CancellationToken ct)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Include(x => x.Account)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.TransactionId == id, ct);
        }
        //В будущем можно сделать IQueryable<Transaction> для отложенного выполнения

        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountId(int id, CancellationToken ct)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Include(x => x.Account)
                .Include(x => x.Category)
                .Where(x=>x.AccountId == id)
                .ToListAsync(ct);
        }
        //реализовать
        public async Task<IEnumerable<Transaction>> GetTransactions(TransactionFilter filter, CancellationToken ct)
        {
            var query = _context.Transactions
                .AsNoTracking()
                .Include(x => x.Account)
                .Include(x => x.Category)
                .Where(x => x.Account.UserId == filter.UserId);

            if (!string.IsNullOrEmpty(filter.Type))
            {
                if (filter.Type == "income")
                {
                    query = query.Where(x => x.Amount > 0);
                }
                else if (filter.Type == "expense")
                {
                    query = query.Where(x => x.Amount < 0);
                }
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(x => x.Date >= filter.StartDate.Value);
            }
            if (filter.EndDate.HasValue)
            {
                query = query.Where(x => x.Date <= filter.EndDate.Value);
            }
            query = ApplySorting(query, filter.SortBy, filter.SortOrder);
            return await query
                //.Skip((filter.Page - 1) * filter.PageSize)
                //.Take(filter.PageSize)
                .ToListAsync(ct);
        }
        private IQueryable<T> ApplyOrder<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector , bool ascending)
        {
            if (ascending)
            {
                return query.OrderBy(keySelector);
            }
            return query.OrderByDescending(keySelector);
        }
        private  IQueryable<Transaction> ApplySorting(IQueryable<Transaction> query,string? sortBy,string? sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return query.OrderByDescending(x => x.Date);
            }
            bool isAscending = true;
            if (sortOrder=="desc")
            {
                isAscending = false;
            }

            return sortBy.ToLower() switch
            {
                "date" => ApplyOrder(query, x => x.Date, isAscending),
                "amount" => ApplyOrder(query, x => x.Amount, isAscending),
                "category" => ApplyOrder(query, x => x.Category.Name, isAscending),
                "account" => ApplyOrder(query, x => x.Account.Name, isAscending),
                _ => ApplyOrder(query, x => x.Date, false) 
            };
        }
        public async Task<FinancialSummary> GetTransactionsSummaryAsync(int userId, int year, int month, CancellationToken ct)
        {
            var result = await _context.Transactions
                .AsNoTracking()
                .Where(x => x.Account.UserId == userId
                         && x.Date.Year == year
                         && x.Date.Month == month)
                .GroupBy(x => 1)
                .Select(g => new FinancialSummary(
                    TotalIncome: g.Where(t => t.Amount > 0).Sum(t => t.Amount),
                    TotalExpenses: g.Where(t => t.Amount < 0).Sum(t => -t.Amount),
                    NetChange: g.Sum(t => t.Amount)
                ))
                .FirstOrDefaultAsync(ct);
            return result ?? new FinancialSummary(0, 0, 0);
        }
       
        public async Task CreateTransaction(Transaction Transaction)
        {
            await _context.Transactions.AddAsync(Transaction);
        }

        public async Task UpdateTransaction(Transaction Transaction)
        {
            _context.Transactions.Update(Transaction);
        }

        public async Task DeleteTransaction(int id)
        {
            var acc = await _context.Transactions.FindAsync(id);
            if (acc != null)
                _context.Transactions.Remove(acc);
        }
    }
}
