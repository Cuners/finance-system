using Auth.Application.UseCases;
using Auth.Domain;
using Auth.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;
        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers(CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<User?> GetUserById(int id, CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User?> GetUserByName(string login, CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(x=>x.Roles)
                .ThenInclude(p=>p.Permissions)
                .Where(x=>x.Login==login)
                .FirstOrDefaultAsync(ct);
        }
        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
        }
    }
}
