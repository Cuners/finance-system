using Finance.Domain;

namespace Finance.Application.UseCases.Budgets.GetBudgetsStatus
{
    public record GetBudgetsStatusResult(IReadOnlyList<BudgetStatus> BudgetStatuses);
}
