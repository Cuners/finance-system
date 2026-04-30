using Finance.Application.Services;
using Finance.Application.UseCases.Accounts.UpdateAccount;
using Finance.Application.UseCases.Accounts.UpdateAccount.Request;
using Finance.Application.UseCases.Accounts.UpdateAccount.Response;
using Finance.Application.UseCases.Transactions.UpdateTransaction.Request;
using Finance.Application.UseCases.Transactions.UpdateTransaction.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.UpdateTransaction
{
    public class UpdateTransactionUseCase : IUseCase<UpdateTransactionRequest,UpdateTransactionResponse>
    {
        private readonly ITransactionRepository _transaction;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<UpdateTransactionUseCase> _logger;
        private readonly IAccountCacheInvalidator _cache;
        private readonly ITransactionCacheInvalidator _cacheTransaction;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserService _currentUserService;
        public UpdateTransactionUseCase(ITransactionRepository transaction,
                                    IUnitOfWork unitOfWork,
                                    ILogger<UpdateTransactionUseCase> logger,
                                    IAccountCacheInvalidator cache,
                                    ITransactionCacheInvalidator cacheTransaction,
                                    IAccountRepository accountRepository,
                                    IEventPublisher eventPublisher,
                                    ICurrentUserService currentUserService)
        {
            _transaction = transaction;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
            _cacheTransaction = cacheTransaction;
            _accountRepository = accountRepository;
            _eventPublisher = eventPublisher;
            _currentUserService = currentUserService;
        }
        public async Task<UpdateTransactionResponse> ExecuteAsync(UpdateTransactionRequest request,int userId, CancellationToken ct)
        {
            try
            {
                var email = _currentUserService.Email;
                var transaction = await _transaction.GetTransactionByTransactionId(request.TransactionId, ct);
                var account = await _accountRepository.GetAccountByAccountId(request.AccountId, ct);
                decimal amount = request.Amount-transaction.Amount;
                transaction.AccountId= request.AccountId;
                transaction.CategoryId = request.CategoryId;
                transaction.Amount= request.Amount;
                transaction.Note = request.Note;
                account.Balance += amount;
                if (request.Amount < 0 && account.Balance < 0)
                {
                    await _eventPublisher.PublishTransactionCreatedAsync(
                        userId,
                        email,
                        account.Name,
                        account.Balance,
                        Math.Abs(request.Amount),
                        transaction.TransactionId,
                        ct);

                    _logger.LogWarning($"Budget exceeded for user {userId}, account {account.Name}, balance {account.Balance}");
                }
                await _accountRepository.UpdateAccount(account);
                await _transaction.UpdateTransaction(transaction);
                await _unitOfWork.SaveChangesAsync(ct);
                await Task.WhenAll(
                    _cache.InvalidateAsync(userId, request.AccountId, ct),
                    _cacheTransaction.InvalidateAsync(userId, request.CategoryId, ct)
                );
                return new UpdateTransactionSuccessResponse(transaction.TransactionId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new UpdateTransactionErrorResponse("Unable to update account at this time", "INVALID_UPDATE");
            }
        }
    }
}
