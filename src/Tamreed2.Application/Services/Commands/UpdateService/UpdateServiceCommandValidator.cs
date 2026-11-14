
using FluentValidation;

namespace Tamreed2.Application.Services.Commands.UpdateService;

public sealed class UpdateServiceCommandValidator
    : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(command => command.ServiceId)
            .NotEmpty()
            .WithMessage("Service ID is required.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Service name is required.")
            .MaximumLength(150)
            .WithMessage(
                "Service name cannot exceed 150 characters.");

        RuleFor(command => command.Description)
            .NotEmpty()
            .WithMessage("Service description is required.")
            .MaximumLength(1000)
            .WithMessage(
                "Service description cannot exceed 1000 characters.");

        RuleFor(command => command.Price)
            .GreaterThan(0)
            .WithMessage(
                "Service price must be greater than zero.");
    }
}
