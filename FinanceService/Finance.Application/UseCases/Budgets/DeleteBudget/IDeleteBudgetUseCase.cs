using Finance.Application.Common;

namespace Finance.Application.UseCases.Budgets.DeleteBudget
{
    public interface IDeleteBudgetUseCase
    {
        Task<Result<DeleteBudgetResult>> ExecuteAsync(
            DeleteBudgetCommand command,
            int userId,
            CancellationToken ct);
    }
}
