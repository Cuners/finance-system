using Finance.Application.DTO;

namespace Finance.Application.UseCases.Budgets.GetBudgetsByUserId
{
    public record GetBudgetsByUserIdResult(IReadOnlyList<BudgetDto> Budgets);
}
