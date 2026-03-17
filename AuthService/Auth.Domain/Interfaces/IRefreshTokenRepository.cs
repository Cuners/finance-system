using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetTokenByJti(string jti, CancellationToken ct);
        Task Create(RefreshToken refreshToken, CancellationToken ct);
    }
}
