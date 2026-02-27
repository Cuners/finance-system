using Auth.Application.UseCases.LoginUser.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.Validators
{
    public class LoginUserRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(x=>x.Username)
                .NotEmpty().WithMessage("User name is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("User name maximum length is 100")
                .Must(name => !name.All(char.IsDigit))
                .WithMessage("User name cannot consist only of digits.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
