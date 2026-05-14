using Finance.Application.Common;

namespace Finance.Application.UseCases.Budgets.СreateBudget
{
    public interface ICreateBudgetUseCase
    {
        Task<Result<CreateBudgetResult>> ExecuteAsync(
            CreateBudgetCommand command,
            int userId,
            CancellationToken ct);
    }
}
