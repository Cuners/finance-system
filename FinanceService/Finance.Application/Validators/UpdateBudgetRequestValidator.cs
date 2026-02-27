using Finance.Application.UseCases.Budgets.UpdateBudget.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Validators
{
    public class UpdateBudgetRequestValidator : AbstractValidator<UpdateBudgetRequest>
    {
        public UpdateBudgetRequestValidator()
        {
            RuleFor(x => x.BudgetId)
                .GreaterThan(0).WithMessage("Account ID is required.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Budget name is required.")
                .MaximumLength(100).WithMessage("Budget name maximum length is 100");

            RuleFor(x => x.LimitAmount)
                .GreaterThan(0).WithMessage("Budget limit must be greater than zero.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Budget date must be for current or future month.");

            RuleFor(x => x.Categories)
                .NotNull().WithMessage("At least one category is required.")
                .Must(list => list.Count >= 1).WithMessage("At least one category is required.");
        }

    }
}
