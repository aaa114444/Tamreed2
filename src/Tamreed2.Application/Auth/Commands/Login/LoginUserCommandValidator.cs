using FluentValidation;

namespace Tamreed2.Application.Auth.Commands.Login;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
 RuleFor(x => x.Email)
     .NotEmpty()
     .WithMessage("Email is required.")
     .EmailAddress()
            .WithMessage("Email format is invalid.");

        RuleFor(x => x.Password)
     .NotEmpty()
     .WithMessage("Password is required.");
    }
}
