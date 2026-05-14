using Finance.Application.Common;

namespace Finance.Application.UseCases.Budgets.GetBudgetsByUserId
{
    public interface IGetBudgetsByUserIdUseCase
    {
        Task<Result<GetBudgetsByUserIdResult>> ExecuteAsync(
            GetBudgetsByUserIdQuery query,
            int userId,
            CancellationToken ct);
    }
}
