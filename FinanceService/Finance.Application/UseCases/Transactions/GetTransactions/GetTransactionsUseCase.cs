using Finance.Application.Common;
using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Finance.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Transactions.GetTransactions
{
    public class GetTransactionsUseCase : IGetTransactionsUseCase
    {
        private readonly ITransactionRepository _transactions;
        private readonly ILogger<GetTransactionsUseCase> _logger;
        private readonly ICacheService _cache;

        public GetTransactionsUseCase(
            ITransactionRepository transactions,
            ILogger<GetTransactionsUseCase> logger,
            ICacheService cache)
        {
            _transactions = transactions;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<GetTransactionsResult>> ExecuteAsync(
            GetTransactionsQuery query,
            int userId,
            CancellationToken ct)
        {
            var filter = new TransactionFilter(
                UserId: userId,
                AccountId: query.AccountId,
                Type: query.Type,
                SortBy: query.SortBy,
                SortOrder: query.SortOrder,
                StartDate: query.StartDate,
                EndDate: query.EndDate);

            var cacheKey = $"transactions:user:{userId}:" +
                           $"from:{query.StartDate:yyyy-MM-dd}:" +
                           $"to:{query.EndDate:yyyy-MM-dd}:" +
                           $"type:{query.Type ?? "all"}:" +
                           $"sort:{query.SortBy ?? "date"}:{query.SortOrder ?? "desc"}";

            try
            {
                var transactions = await _cache.GetOrCreateAsync(cacheKey, async token =>
                {
                    return await BuildTransactionsAsync(filter, token);
                });

                return Result<GetTransactionsResult>.Success(
                    new GetTransactionsResult(transactions ?? []));
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning(ex, "Cache timeout while getting transactions for user {UserId}", userId);
                var transactions = await BuildTransactionsAsync(filter, ct);
                return Result<GetTransactionsResult>.Success(new GetTransactionsResult(transactions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get transactions for user {UserId}", userId);
                return Result<GetTransactionsResult>.Failure(
                    "TRANSACTIONS_GET_FAILED",
                    "Unable to get transactions at this time");
            }
        }

        private async Task<IReadOnlyList<TransactionDto>> BuildTransactionsAsync(
            TransactionFilter filter,
            CancellationToken ct)
        {
            var transactions = await _transactions.GetTransactions(filter, ct);
            return transactions.Select(ToDto).ToList();
        }

        private static TransactionDto ToDto(Domain.Transaction transaction)
        {
            return new TransactionDto(
                transaction.TransactionId,
                transaction.AccountId,
                transaction.Account.Name,
                transaction.CategoryId,
                transaction.Category.Name,
                transaction.Amount,
                transaction.Date,
                transaction.Note);
        }
    }
}
