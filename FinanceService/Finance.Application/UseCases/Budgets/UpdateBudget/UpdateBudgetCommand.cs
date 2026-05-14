namespace Finance.Application.UseCases.Budgets.UpdateBudget
{
    public record UpdateBudgetCommand(
        int BudgetId,
        string Name,
        decimal LimitAmount,
        int CategoryId,
        DateOnly Date);
}
