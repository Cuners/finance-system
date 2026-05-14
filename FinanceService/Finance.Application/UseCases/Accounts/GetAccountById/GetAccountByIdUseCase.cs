using Finance.Application.Common;
using Finance.Application.DTO;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Accounts.GetAccountById
{
    public class GetAccountByIdUseCase : IGetAccountByIdUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<GetAccountByIdUseCase> _logger;

        public GetAccountByIdUseCase(
            IAccountRepository accountRepository,
            ILogger<GetAccountByIdUseCase> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<Result<GetAccountByIdResult>> ExecuteAsync(
            GetAccountByIdQuery query,
            int userId,
            CancellationToken ct)
        {
            try
            {
                if (query.AccountId <= 0)
                {
                    return Result<GetAccountByIdResult>.Failure("INVALID_ACCOUNT_ID", "Invalid account id");
                }

                var account = await _accountRepository.GetAccountByAccountId(query.AccountId, ct);
                if (account == null || account.UserId != userId)
                {
                    return Result<GetAccountByIdResult>.Failure(
                        "ACCOUNT_ACCESS_DENIED",
                        "No account found or access denied");
                }

                var result = new AccountDto
                {
                    AccountId = account.AccountId,
                    Name = account.Name,
                    Balance = account.Balance,
                    Note = account.Note
                };

                return Result<GetAccountByIdResult>.Success(new GetAccountByIdResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get account {AccountId} for user {UserId}", query.AccountId, userId);
                return Result<GetAccountByIdResult>.Failure(
                    "ACCOUNT_GET_FAILED",
                    "Unable to get account at this time");
            }
        }
    }
}
