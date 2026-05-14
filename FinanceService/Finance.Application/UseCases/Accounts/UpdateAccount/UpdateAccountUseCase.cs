using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Accounts.UpdateAccount
{
    public class UpdateAccountUseCase : IUpdateAccountUseCase
    {
        private readonly IAccountRepository _account;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateAccountUseCase> _logger;
        private readonly IAccountCacheInvalidator _cache;

        public UpdateAccountUseCase(
            IAccountRepository account,
            IUnitOfWork unitOfWork,
            ILogger<UpdateAccountUseCase> logger,
            IAccountCacheInvalidator cache)
        {
            _account = account;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<UpdateAccountResult>> ExecuteAsync(
            UpdateAccountCommand command,
            int userId,
            CancellationToken ct)
        {
            try
            {
                var account = await _account.GetAccountByAccountId(command.AccountId, ct);
                if (account == null || account.UserId != userId)
                {
                    return Result<UpdateAccountResult>.Failure(
                        "ACCOUNT_ACCESS_DENIED",
                        "Account not found or access denied");
                }

                account.Balance = command.Balance;
                account.Name = command.Name;
                account.Note = command.Note;

                await _account.UpdateAccount(account);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, command.AccountId, ct);

                return Result<UpdateAccountResult>.Success(new UpdateAccountResult(account.AccountId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update account {AccountId} for user {UserId}", command.AccountId, userId);
                return Result<UpdateAccountResult>.Failure(
                    "ACCOUNT_UPDATE_FAILED",
                    "Unable to update account at this time");
            }
        }
    }
}
