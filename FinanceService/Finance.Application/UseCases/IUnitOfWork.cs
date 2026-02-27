using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken ct);
    }
}
