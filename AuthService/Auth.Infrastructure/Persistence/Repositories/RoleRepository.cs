using Auth.Domain;
using Auth.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AuthDbContext _context;
        public RoleRepository(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Role>> GetAllRoles(CancellationToken ct)
        {
            return await _context.Roles
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Role?> GetRoleById(int Id, CancellationToken ct)
        {
            return await _context.Roles
                .AsNoTracking()
                .Include(x => x.Permissions)
                .Where(x => x.RoleId == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Role>> GetRolesById(List<int> ids, CancellationToken ct)
        {
            return await _context.Roles
                .AsNoTracking()
                .Where(x => ids.Contains(x.RoleId))
                .ToListAsync();
        }
    }
}
