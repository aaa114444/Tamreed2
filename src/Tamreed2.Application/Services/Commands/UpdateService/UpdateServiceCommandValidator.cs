using FluentValidation;

namespace Tamreed2.Application.Services.Commands.UpdateService;

public sealed class UpdateServiceCommandValidator
    : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(command => command.ServiceId).NotEmpty();

        RuleFor(command => command)
            .Must(command => !string.IsNullOrWhiteSpace(command.Title) || !string.IsNullOrWhiteSpace(command.TitleAr))
            .WithMessage("Service title is required in at least one language.");
        RuleFor(command => command.Title).MaximumLength(200);
        RuleFor(command => command.TitleAr).MaximumLength(200);

        RuleFor(command => command)
            .Must(command => !string.IsNullOrWhiteSpace(command.Name) || !string.IsNullOrWhiteSpace(command.NameAr))
            .WithMessage("Service name is required in at least one language.");
        RuleFor(command => command.Name).MaximumLength(150);
        RuleFor(command => command.NameAr).MaximumLength(150);

        RuleFor(command => command)
            .Must(command => !string.IsNullOrWhiteSpace(command.Description) || !string.IsNullOrWhiteSpace(command.DescriptionAr))
            .WithMessage("Service description is required in at least one language.");
        RuleFor(command => command.Description).MaximumLength(1000);
        RuleFor(command => command.DescriptionAr).MaximumLength(1000);

        RuleFor(command => command.Points)
            .Must(points => points == null || points.Count <= 20)
            .WithMessage("A service cannot have more than 20 points.");
        RuleForEach(command => command.Points)
            .NotEmpty().MaximumLength(200);

        RuleFor(command => command.PointsAr)
            .Must(points => points == null || points.Count <= 20)
            .WithMessage("A service cannot have more than 20 Arabic points.");
        RuleForEach(command => command.PointsAr)
            .NotEmpty().MaximumLength(200);
    }
}