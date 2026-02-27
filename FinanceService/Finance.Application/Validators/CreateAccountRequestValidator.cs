using Finance.Application.UseCases.Accounts.CreateAccount.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Validators
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest> 
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Account name is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("Account name maximum length is 100")
                .Must(name => !name.All(char.IsDigit))
                .WithMessage("Account name cannot consist only of digits.");

            RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("Initial balance cannot be negative.")
                .LessThanOrEqualTo(100_000_000).WithMessage("Initial balance cannot be more than 100 000 000.");
        }
    }
}
