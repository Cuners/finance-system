using Finance.Application.Services;
using Finance.Application.UseCases.Transactions.GetTransactions.Request;
using Finance.Application.UseCases.Transactions.GetTransactions.Response;
using Finance.Domain.Interfaces;
using Finance.Domain.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.GetTransactions
{
    public class GetTransactionsUseCase : IUseCase<GetTransactionsRequest, GetTransactionsResponse>
    {
        private readonly ITransactionRepository _transaction;
        private readonly ILogger<GetTransactionsUseCase> _logger;
        private readonly ICacheService _cache;
        public GetTransactionsUseCase(ITransactionRepository transaction, ILogger<GetTransactionsUseCase> logger, ICacheService cache)
        {
            _transaction = transaction;
            _logger = logger;
            _cache = cache;
        }
        //Получать через request userid а в контроллере как раз и расшифровывать токен
        public async Task<GetTransactionsResponse> ExecuteAsync(GetTransactionsRequest request, CancellationToken ct)
        {
            if (request.UserId <= 0)
            {
                _logger.LogWarning("GetBudgetRequest is null");
                return new GetTransactionsErrorResponse("Invalid User id", "INVALID_USER_ID");
            }
            var filter = new TransactionFilter
            (
                UserId: request.UserId,
                AccountId: request.AccountId,
                Type: request.Type,
                SortBy: request.SortBy,
                SortOrder: request.SortOrder,
                StartDate: request.StartDate,
                EndDate: request.EndDate
            );
            var cacheKey = $"transactions:user:{request.UserId}:" +
              $"from:{request.StartDate:yyyy-MM-dd}:" +
              $"to:{request.EndDate:yyyy-MM-dd}:" +
              $"type:{request.Type ?? "all"}:" +
              $"sort:{request.SortBy ?? "date"}:{request.SortOrder ?? "desc"}";
            try
            {
                var transactions = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    var transactions = await _transaction.GetTransactions(filter, ct);
                    var result = transactions.Select(x => new TransactionDto
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
                    return result;
                });
                
                if (transactions is null)
                {
                    return new GetTransactionsErrorResponse("Unable to get Transactions at this time", "INVALID_GET");
                }
                return new GetTransactionsSuccessResponse(transactions);
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, $"Cache lock timeout for key {cacheKey}");
                var Transactions = await _transaction.GetTransactions(filter, ct);
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
                return new GetTransactionsSuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetTransactionsErrorResponse("Unable to get Transactions at this time", "INVALID_GET");
            }
        }
    }
}
