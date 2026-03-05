using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId.Request;
using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactionsByAccountId
{
    public class GetTransactionsByAccountIdUseCase : IUseCase<GetTransactionsByAccountIdRequest, GetTransactionsByAccountIdResponse>
    {
        private readonly ITransactionRepository _Transaction;
        private readonly ILogger<GetTransactionsByAccountIdUseCase> _logger;
        public GetTransactionsByAccountIdUseCase(ITransactionRepository Transaction, ILogger<GetTransactionsByAccountIdUseCase> logger)
        {
            _Transaction = Transaction;
            _logger = logger;
        }
        public async Task<GetTransactionsByAccountIdResponse> ExecuteAsync(GetTransactionsByAccountIdRequest request, CancellationToken ct)
        {
            try
            {
                var Transactions = await _Transaction.GetTransactionsByAccountId(request.AccountId,ct);
                var result = Transactions.Select(x => new TransactionDto
                (
                    x.TransactionId,
                    x.AccountId,
                    x.Account.Name,
                    x.CategoryId,
                    x.Category.Name,
                    x.Amount,
                    x.Date,
                    x.Note
                )).ToList();
                return new GetTransactionsByAccountIdSuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetTransactionsByAccountIdErrorResponse("Unable to get Transactions at this time", "INVALID_UPDATE");
            }
        }
    }
}
