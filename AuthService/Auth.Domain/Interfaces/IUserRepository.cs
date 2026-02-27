using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers(CancellationToken ct);
        Task<User?> GetUserById(int id, CancellationToken ct);
        Task CreateUser(User user);
        Task<User?> GetUserByName(string login, CancellationToken ct);
    }
}
