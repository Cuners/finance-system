using Finance.Application.Services;
using Finance.Application.UseCases.Transactions.CreateTransaction.Request;
using Finance.Application.UseCases.Transactions.CreateTransaction.Response;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.UseCases.Transactions.CreateTransaction
{
    public class CreateTransactionUseCase : IUseCase<CreateTransactionRequest, CreateTransactionResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateTransactionUseCase> _logger;
        private readonly ITransactionCacheInvalidator _cache;
     //   private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserService _currentUserService;
        public CreateTransactionUseCase(ITransactionRepository transactionRepository,
                                        IAccountRepository accountRepository,
                                        ICategoryRepository categoryRepository,
                                        IUnitOfWork unitOfWork,
                                        ILogger<CreateTransactionUseCase> logger,
                                        ITransactionCacheInvalidator cache,
                                       // IEventPublisher eventPublisher,
                                        ICurrentUserService currentUserService)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
          //  _eventPublisher = eventPublisher;
            _currentUserService = currentUserService;
        }

        public async Task<CreateTransactionResponse> ExecuteAsync(CreateTransactionRequest request, int userId, CancellationToken ct)
        {
            try
            {
                var email = _currentUserService.Email;
                var account = await _accountRepository.GetAccountByAccountId(request.AccountId,ct);
                if (account == null)
                {
                    return new CreateTransactionErrorResponse("Account not found or access denied", "ACCOUNT_NOT_FOUND");
                }
                var category = await _categoryRepository.GetCategoryById(request.CategoryId,ct);
                if (category == null)
                {
                    return new CreateTransactionErrorResponse("Category not found", "CATEGORY_NOT_FOUND");
                }

                var transaction = new Domain.Transaction
                {
                    AccountId = request.AccountId,
                    CategoryId = request.CategoryId,
                    Amount = request.Amount,
                    Date = request.Date,
                    Note = request.Note
                };
                account.Balance += request.Amount;
                await _transactionRepository.CreateTransaction(transaction);
                await _accountRepository.UpdateAccount(account);
                await _unitOfWork.SaveChangesAsync(ct);
                //if (request.Amount < 0 && account.Balance < 0)
                //{
                //    await _eventPublisher.PublishBudgetExceededAsync(
                //        userId,
                //        account.Name,
                //        email,
                //        account.Balance,
                //        Math.Abs(request.Amount),
                //        transaction.TransactionId,
                //        ct);

                //    _logger.LogWarning($"Budget exceeded for user {userId}, account {account.Name}, balance {account.Balance}");
                //}
                
                await _cache.InvalidateAsync(userId,transaction.TransactionId, ct);
                return new CreateTransactionSuccessRepsonse(transaction.TransactionId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error creating transaction for user");
                return new CreateTransactionErrorResponse("Unable to create transaction", "INTERNAL_ERROR");
            }
        }
    }
}
