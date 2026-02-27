using Auth.Domain;
using Auth.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AuthDbContext _context;
        public PermissionRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<Permission?> GetPermissionById(int id, CancellationToken ct)
        {
            return await _context.Permissions
                .AsNoTracking()
                .Where(x=>x.PermissionId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsById(List<int> ids, CancellationToken ct)
        {
            return await _context.Permissions
               .AsNoTracking()
               .Where(x => ids.Contains(x.PermissionId))
               .ToListAsync();
        }
        public async Task<List<string?>> GetPermissionsByRoles(List<string> roles, CancellationToken ct)
        {
            return await _context.Roles
                .AsNoTracking()
                .Where(r => roles.Contains(r.RoleName))
                .SelectMany(r => r.Permissions)
                .Select(p => p.PermissionName)
                .Distinct()
                .ToListAsync();
        }
    }
}
