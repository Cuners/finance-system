using Finance.Domain;
using Finance.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BudgetDbContext _context;
        public CategoryRepository(BudgetDbContext context)
        {
            _context = context;
        }
        public async Task<Category?> GetCategoryById(int id, CancellationToken ct)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoryId == id, ct);
        }
        public async Task<IEnumerable<Category>> GetCategoriesById(List<int> ids, CancellationToken ct)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(x => ids.Contains(x.CategoryId))
                .ToListAsync(ct);
        }
        public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken ct)
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
