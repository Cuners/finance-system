using System;
using System.Collections.Generic;
using System.Text;
namespace Auth.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRoles(CancellationToken ct);
        Task<Role?> GetRoleById(int Id, CancellationToken ct);
        Task<IEnumerable<Role>> GetRolesById(List<int> ids, CancellationToken ct);
        Task<Role?> GetRoleByName(string name, CancellationToken ct);
        Task Create(Role role, CancellationToken ct);
    }
}
