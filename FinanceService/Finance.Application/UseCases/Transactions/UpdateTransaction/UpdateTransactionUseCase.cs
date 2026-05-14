using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Transactions.UpdateTransaction
{
    public class UpdateTransactionUseCase : IUpdateTransactionUseCase
    {
        private readonly ITransactionRepository _transactions;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accounts;
        private readonly ILogger<UpdateTransactionUseCase> _logger;
        private readonly IAccountCacheInvalidator _accountCache;
        private readonly ITransactionCacheInvalidator _transactionCache;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTransactionUseCase(
            ITransactionRepository transactions,
            IUnitOfWork unitOfWork,
            ILogger<UpdateTransactionUseCase> logger,
            IAccountCacheInvalidator accountCache,
            ITransactionCacheInvalidator transactionCache,
            IAccountRepository accounts,
            IEventPublisher eventPublisher,
            ICurrentUserService currentUserService)
        {
            _transactions = transactions;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _accountCache = accountCache;
            _transactionCache = transactionCache;
            _accounts = accounts;
            _eventPublisher = eventPublisher;
            _currentUserService = currentUserService;
        }

        public async Task<Result<UpdateTransactionResult>> ExecuteAsync(
            UpdateTransactionCommand command,
            int userId,
            CancellationToken ct)
        {
            try
            {
                var email = _currentUserService.Email;
                var transaction = await _transactions.GetTransactionByTransactionId(command.TransactionId, ct);
                if (transaction == null)
                {
                    return Result<UpdateTransactionResult>.Failure(
                        "TRANSACTION_NOT_FOUND",
                        "Transaction not found");
                }

                var account = await _accounts.GetAccountByAccountId(command.AccountId, ct);
                if (account == null || account.UserId != userId)
                {
                    return Result<UpdateTransactionResult>.Failure(
                        "ACCOUNT_ACCESS_DENIED",
                        "Account not found or access denied");
                }

                var amountDifference = command.Amount - transaction.Amount;
                transaction.AccountId = command.AccountId;
                transaction.CategoryId = command.CategoryId;
                transaction.Amount = command.Amount;
                transaction.Date = command.Date;
                transaction.Note = command.Note;
                account.Balance += amountDifference;

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

                await _accounts.UpdateAccount(account);
                await _transactions.UpdateTransaction(transaction);
                await _unitOfWork.SaveChangesAsync(ct);
                await Task.WhenAll(
                    _accountCache.InvalidateAsync(userId, command.AccountId, ct),
                    _transactionCache.InvalidateAsync(userId, command.CategoryId, ct));

                return Result<UpdateTransactionResult>.Success(new UpdateTransactionResult(transaction.TransactionId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update transaction {TransactionId} for user {UserId}", command.TransactionId, userId);
                return Result<UpdateTransactionResult>.Failure(
                    "TRANSACTION_UPDATE_FAILED",
                    "Unable to update transaction at this time");
            }
        }
    }
}
