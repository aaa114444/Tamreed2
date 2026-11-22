using FluentValidation;
using Tamreed2.Domain.CallbackRequests;

namespace Tamreed2.Application.CallbackRequests.Commands.UpdateCallbackRequestStatus;

public sealed class UpdateCallbackRequestStatusCommandValidator
    : AbstractValidator<UpdateCallbackRequestStatusCommand>
{
    public UpdateCallbackRequestStatusCommandValidator()
    {
        RuleFor(command => command.RequestId)
            .NotEmpty()
            .WithMessage("Callback request ID is required.");

        RuleFor(command => command.Status)
            .IsInEnum()
            .WithMessage("Select a valid callback request status.");
    }
}
