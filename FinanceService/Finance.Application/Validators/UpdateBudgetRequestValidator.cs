using Finance.Application.UseCases.Budgets.UpdateBudget;
using FluentValidation;

namespace Finance.Application.Validators
{
    public class UpdateBudgetRequestValidator : AbstractValidator<UpdateBudgetCommand>
    {
        public UpdateBudgetRequestValidator()
        {
            RuleFor(x => x.BudgetId)
                .GreaterThan(0).WithMessage("Budget ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Budget name is required.")
                .MaximumLength(100).WithMessage("Budget name maximum length is 100");

            RuleFor(x => x.LimitAmount)
                .GreaterThan(0).WithMessage("Budget limit must be greater than zero.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID is required.");
        }
    }
}
