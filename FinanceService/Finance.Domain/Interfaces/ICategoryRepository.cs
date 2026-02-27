using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesById(List<int> ids, CancellationToken ct);
        Task<IEnumerable<Category>> GetAllCategories(CancellationToken ct);
        Task<Category?> GetCategoryById(int id, CancellationToken ct);
    }
}
