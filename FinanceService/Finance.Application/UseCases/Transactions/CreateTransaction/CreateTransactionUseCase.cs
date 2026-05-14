using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Transactions.CreateTransaction
{
    public class CreateTransactionUseCase : ICreateTransactionUseCase
    {
        private readonly ITransactionRepository _transactions;
        private readonly IAccountRepository _accounts;
        private readonly ICategoryRepository _categories;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateTransactionUseCase> _logger;
        private readonly ITransactionCacheInvalidator _cache;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserService _currentUserService;

        public CreateTransactionUseCase(
            ITransactionRepository transactions,
            IAccountRepository accounts,
            ICategoryRepository categories,
            IUnitOfWork unitOfWork,
            ILogger<CreateTransactionUseCase> logger,
            ITransactionCacheInvalidator cache,
            IEventPublisher eventPublisher,
            ICurrentUserService currentUserService)
        {
            _transactions = transactions;
            _accounts = accounts;
            _categories = categories;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
            _eventPublisher = eventPublisher;
            _currentUserService = currentUserService;
        }

        public async Task<Result<CreateTransactionResult>> ExecuteAsync(
            CreateTransactionCommand command,
            int userId,
            CancellationToken ct)
        {
            try
            {
                var email = _currentUserService.Email;
                var account = await _accounts.GetAccountByAccountId(command.AccountId, ct);
                if (account == null || account.UserId != userId)
                {
                    return Result<CreateTransactionResult>.Failure(
                        "ACCOUNT_ACCESS_DENIED",
                        "Account not found or access denied");
                }

                var category = await _categories.GetCategoryById(command.CategoryId, ct);
                if (category == null)
                {
                    return Result<CreateTransactionResult>.Failure("CATEGORY_NOT_FOUND", "Category not found");
                }

                var transaction = new Domain.Transaction
                {
                    AccountId = command.AccountId,
                    CategoryId = command.CategoryId,
                    Amount = command.Amount,
                    Date = command.Date,
                    Note = command.Note
                };

                account.Balance += command.Amount;
                await _transactions.CreateTransaction(transaction);
                await _accounts.UpdateAccount(account);
                await _unitOfWork.SaveChangesAsync(ct);

                if (command.Amount < 0 && account.Balance < 0)
                {
                    await _eventPublisher.PublishTransactionCreatedAsync(
                        userId,
                        email,
                        account.Name,
                        account.Balance,
                        Math.Abs(command.Amount),
                        transaction.TransactionId,
                        ct);

                    _logger.LogWarning(
                        "Budget exceeded for user {UserId}, account {AccountName}, balance {Balance}",
                        userId,
                        account.Name,
                        account.Balance);
                }

                await _cache.InvalidateAsync(userId, transaction.TransactionId, ct);
                return Result<CreateTransactionResult>.Success(new CreateTransactionResult(transaction.TransactionId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create transaction for user {UserId}", userId);
                return Result<CreateTransactionResult>.Failure(
                    "TRANSACTION_CREATE_FAILED",
                    "Unable to create transaction at this time");
            }
        }
    }
}
