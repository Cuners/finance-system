using Finance.Application.Common;

namespace Finance.Application.UseCases.Budgets.GetBudgetsSummary
{
    public interface IGetBudgetsSummaryUseCase
    {
        Task<Result<GetBudgetsSummaryResult>> ExecuteAsync(
            GetBudgetsSummaryQuery query,
            int userId,
            CancellationToken ct);
    }
}
