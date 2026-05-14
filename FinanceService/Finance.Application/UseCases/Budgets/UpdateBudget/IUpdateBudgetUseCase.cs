using Finance.Application.Common;

namespace Finance.Application.UseCases.Budgets.UpdateBudget
{
    public interface IUpdateBudgetUseCase
    {
        Task<Result<UpdateBudgetResult>> ExecuteAsync(
            UpdateBudgetCommand command,
            int userId,
            CancellationToken ct);
    }
}
