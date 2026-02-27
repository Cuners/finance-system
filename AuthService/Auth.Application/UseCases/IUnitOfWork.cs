using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Auth.Application.UseCases
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken ct);
    }
}
