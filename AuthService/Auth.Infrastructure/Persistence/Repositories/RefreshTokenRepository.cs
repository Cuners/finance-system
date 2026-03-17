using Auth.Domain;
using Auth.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthDbContext _context;
        public RefreshTokenRepository(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<RefreshToken?> GetTokenByJti(string jti,CancellationToken ct)
        {
            return await _context.RefreshTokens
                         .Where(x => x.Jti == jti)
                         .FirstOrDefaultAsync(ct);
        }

        public async Task Create(RefreshToken refreshToken, CancellationToken ct)
        {
            await _context.RefreshTokens
                  .AddAsync(refreshToken, ct);
        }
    }
}
