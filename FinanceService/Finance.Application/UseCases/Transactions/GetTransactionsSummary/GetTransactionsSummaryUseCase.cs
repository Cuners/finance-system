
using Finance.Application.Services;
using Finance.Application.UseCases.Transactions.GetTransactionsSummary.Request;
using Finance.Application.UseCases.Transactions.GetTransactionsSummary.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Finance.Application.UseCases.Transactions.GetTransactionsSummary
{
    public class GetTransactionsSummaryUseCase : IUseCase<GetTransactionsSummaryRequest, GetTransactionSummaryResponse>
    {
        private readonly ITransactionRepository _Transaction;
        private readonly ILogger<GetTransactionsSummaryUseCase> _logger;
        private readonly ICacheService _cache;
        public GetTransactionsSummaryUseCase(ITransactionRepository Transaction, ILogger<GetTransactionsSummaryUseCase> logger,
                                             ICacheService cache)
        {
            _Transaction = Transaction;
            _logger = logger;
            _cache = cache;
        }
        //Получать через request userid а в контроллере как раз и расшифровывать токен
        public async Task<GetTransactionSummaryResponse> ExecuteAsync(GetTransactionsSummaryRequest request,int userId, CancellationToken ct)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("GetBudgetRequest is null");
                return new GetTransactionSummaryErrorResponse("Invalid User id", "INVALID_USER_ID");
            }
            var cacheKey = $"dashboard:user:{userId}:" +
                           "transactionsSummary:" +
                           $"from:{request.Year}-{request.Month:D2}";
            try
            {
                var transactions = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    var summary = await _Transaction.GetTransactionsSummaryAsync(userId, request.Year, request.Month, ct);
                    var sumTrans = new TransactionSummaryDto(TotalIncome: summary.TotalIncome,
                                                             TotalExpenses: summary.TotalExpenses,
                                                             NetBalance: summary.NetChange);
                    return sumTrans;
                });

                if (transactions is null)
                {
                    return new GetTransactionSummaryErrorResponse("Unable to get Sum for Transactions at this time", "INVALID_GET");
                }
                return new GetTransactionSummarySuccessResponse(transactions);
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, $"Cache lock timeout for key {cacheKey}");
                var summary = await _Transaction.GetTransactionsSummaryAsync(userId, request.Year, request.Month, ct);
                var sumTrans = new TransactionSummaryDto(TotalIncome: summary.TotalIncome,
                                                         TotalExpenses: summary.TotalExpenses,
                                                         NetBalance: summary.NetChange);
                return new GetTransactionSummarySuccessResponse(sumTrans);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new GetTransactionSummaryErrorResponse("Unable to get Sum for Transactions at this time", "INVALID_GET");
            }
        }
    }
}
