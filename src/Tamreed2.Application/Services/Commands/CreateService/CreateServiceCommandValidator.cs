
using FluentValidation;

namespace Tamreed2.Application.Services.Commands.CreateService;

public sealed class CreateServiceCommandValidator
    : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Service name is required.")
            .MaximumLength(150)
            .WithMessage("Service name cannot exceed 150 characters.");

        RuleFor(command => command.Description)
            .NotEmpty()
            .WithMessage("Service description is required.")
            .MaximumLength(1000)
            .WithMessage("Service description cannot exceed 1000 characters.");

        RuleFor(command => command.Price)
            .GreaterThan(0)
            .WithMessage("Service price must be greater than zero.");
    }
}
