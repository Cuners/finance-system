using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases
{
    public interface IUseCase <TRequest, TResponse>
    {
         Task<TResponse> ExecuteAsync(TRequest request,CancellationToken ct);
    }
}
