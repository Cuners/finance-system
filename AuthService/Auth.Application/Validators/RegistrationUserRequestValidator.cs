using Auth.Application.UseCases.RegistrateUser.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.Validators
{
    public class RegistrationUserRequestValidator : AbstractValidator<RegistrationRequest>
    {
        public RegistrationUserRequestValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("User name is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("User name maximum length is 100")
                .Must(name => !name.All(char.IsDigit))
                .WithMessage("User name cannot consist only of digits.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("Email maximum length is 100");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
