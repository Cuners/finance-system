using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Accounts.CreateAccount
{
    public class CreateAccountUseCase : ICreateAccountUseCase
    {
        private readonly IAccountRepository _accounts;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateAccountUseCase> _logger;
        private readonly IAccountCacheInvalidator _cache;

        public CreateAccountUseCase(
            IAccountRepository accounts,
            IUnitOfWork unitOfWork,
            ILogger<CreateAccountUseCase> logger,
            IAccountCacheInvalidator cache)
        {
            _accounts = accounts;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<CreateAccountResult>> ExecuteAsync(
            CreateAccountCommand command,
            int userId,
            CancellationToken ct)
        {
            try
            {
                var account = new Domain.Account
                {
                    UserId = userId,
                    Name = command.Name,
                    Balance = command.Balance,
                    Note = command.Note
                };

                await _accounts.CreateAccount(account);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, account.AccountId, ct);

                return Result<CreateAccountResult>.Success(new CreateAccountResult(account.AccountId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create account for user {UserId}", userId);
                return Result<CreateAccountResult>.Failure(
                    "ACCOUNT_CREATE_FAILED",
                    "Unable to create account at this time");
            }
        }
    }
}
