using Finance.Application.DTO;

namespace Finance.Application.UseCases.Accounts.GetAccountsByUserId
{
    public record GetAccountsByUserIdResult(IReadOnlyList<AccountSummaryDto> Accounts);
}
