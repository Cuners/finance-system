using Finance.Application.Common;

namespace Finance.Application.UseCases.Budgets.GetBudgetById
{
    public interface IGetBudgetByIdUseCase
    {
        Task<Result<GetBudgetByIdResult>> ExecuteAsync(
            GetBudgetByIdQuery query,
            int userId,
            CancellationToken ct);
    }
}
