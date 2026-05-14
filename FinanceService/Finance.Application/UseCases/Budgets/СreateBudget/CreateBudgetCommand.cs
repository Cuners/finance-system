namespace Finance.Application.UseCases.Budgets.СreateBudget
{
    public record CreateBudgetCommand(
        string Name,
        decimal LimitAmount,
        int CategoryId);
}
