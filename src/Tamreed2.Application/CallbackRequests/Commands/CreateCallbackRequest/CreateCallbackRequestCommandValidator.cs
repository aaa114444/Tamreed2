using FluentValidation;

namespace Tamreed2.Application.CallbackRequests.Commands.CreateCallbackRequest;

public sealed class CreateCallbackRequestCommandValidator
    : AbstractValidator<CreateCallbackRequestCommand>
{
    public CreateCallbackRequestCommandValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(120)
            .WithMessage("Name cannot exceed 120 characters.");

        RuleFor(request => request.MobileNumber)
            .NotEmpty()
            .WithMessage("Mobile number is required.")
            .MaximumLength(30)
            .WithMessage("Mobile number cannot exceed 30 characters.")
            .Matches(@"^[+0-9\s()-]{7,30}$")
            .WithMessage("Enter a valid mobile number.");

        RuleFor(request => request.ServicesNeeded)
            .NotNull()
            .WithMessage("Services are required.")
            .Must(services =>
                services is not null &&
                services.Count >= 1 &&
                services.Count <= 10)
            .WithMessage("Select between 1 and 10 services.");

        RuleForEach(request => request.ServicesNeeded)
            .NotEmpty()
            .WithMessage("A selected service cannot be empty.")
            .MaximumLength(150)
            .WithMessage("A service name cannot exceed 150 characters.");

        RuleFor(request => request.Message)
            .MaximumLength(1000)
            .WithMessage("Message cannot exceed 1000 characters.");
    }
}
