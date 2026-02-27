using Finance.Application.UseCases.Transactions.CreateTransaction.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Validators
{
    public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionRequestValidator()
        {
            RuleFor(x => x.AccountId)
                .GreaterThan(0).WithMessage("Account ID is required.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID is required.");

            RuleFor(x => x.Amount)
                .NotEqual(0).WithMessage("Transaction amount cannot be zero.")
                .GreaterThan(-10_000_000).WithMessage("Expense amount is too large.") 
                .LessThan(10_000_000).WithMessage("Income amount is too large.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .Must(BeValidDate).WithMessage("Transaction date cannot be in the future.");

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Transaction note maximum length is 500");
        }

        private bool BeValidDate(DateOnly date)
        {
            return date.ToDateTime(TimeOnly.MinValue) <= DateTime.UtcNow.AddDays(1);
        }
    }
}
