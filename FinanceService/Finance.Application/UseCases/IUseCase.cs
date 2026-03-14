using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases
{
    public interface IUseCase<TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request, int userId, CancellationToken ct);
    }
}
