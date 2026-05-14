using Finance.Application.Common;

namespace Finance.Application.UseCases.Budgets.GetBudgetsStatus
{
    public interface IGetBudgetsStatusUseCase
    {
        Task<Result<GetBudgetsStatusResult>> ExecuteAsync(
            GetBudgetsStatusQuery query,
            int userId,
            CancellationToken ct);
    }
}
