using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByAccountId(int id, CancellationToken ct);
        Task<IEnumerable<Account>> GetAllAccounts(CancellationToken ct);
        Task<IEnumerable<Account>> GetAccountsByUserId(int userId, CancellationToken ct);
        Task<decimal> GetTotalValue(int userid, CancellationToken ct);
        Task CreateAccount(Account account);
        Task UpdateAccount(Account account);
        Task DeleteAccount(int id);
    }
}
