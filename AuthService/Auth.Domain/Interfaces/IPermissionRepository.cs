using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permission?> GetPermissionById(int id, CancellationToken ct);
        Task<IEnumerable<Permission>> GetPermissionsById(List<int> ids, CancellationToken ct);
    }
}
