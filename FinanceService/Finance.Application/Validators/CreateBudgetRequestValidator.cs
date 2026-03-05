using Finance.Application.UseCases.Budgets.СreateBudget.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Validators
{
    public class CreateBudgetRequestValidator : AbstractValidator<CreateBudgetRequest>
    {
        public CreateBudgetRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Budget name is required.")
                .MaximumLength(100).WithMessage("Budget name maximum length is 100");

            RuleFor(x => x.LimitAmount)
                .GreaterThan(0).WithMessage("Budget limit must be greater than zero.");


            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.");
        }

    }
}
