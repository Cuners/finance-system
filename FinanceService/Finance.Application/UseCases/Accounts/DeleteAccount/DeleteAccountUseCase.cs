using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Accounts.DeleteAccount
{
    public class DeleteAccountUseCase : IDeleteAccountUseCase
    {
        private readonly IAccountRepository _account;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteAccountUseCase> _logger;
        private readonly IAccountCacheInvalidator _cache;

        public DeleteAccountUseCase(
            IAccountRepository account,
            IUnitOfWork unitOfWork,
            ILogger<DeleteAccountUseCase> logger,
            IAccountCacheInvalidator cache)
        {
            _account = account;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<DeleteAccountResult>> ExecuteAsync(
            DeleteAccountCommand command,
            int userId,
            CancellationToken ct)
        {
            try
            {
                var account = await _account.GetAccountByAccountId(command.AccountId, ct);
                if (account == null || account.UserId != userId)
                {
                    return Result<DeleteAccountResult>.Failure(
                        "ACCOUNT_ACCESS_DENIED",
                        "Account not found or access denied");
                }

                await _account.DeleteAccount(command.AccountId);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, command.AccountId, ct);

                return Result<DeleteAccountResult>.Success(new DeleteAccountResult(command.AccountId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete account {AccountId} for user {UserId}", command.AccountId, userId);
                return Result<DeleteAccountResult>.Failure(
                    "ACCOUNT_DELETE_FAILED",
                    "Unable to delete account at this time");
            }
        }
    }
}
