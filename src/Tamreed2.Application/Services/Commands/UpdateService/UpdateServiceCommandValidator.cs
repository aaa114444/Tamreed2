using FluentValidation;

namespace Tamreed2.Application.Services.Commands.UpdateService;

public sealed class UpdateServiceCommandValidator
    : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(command => command.ServiceId).NotEmpty();

        RuleFor(command => command.Title).NotEmpty().MaximumLength(200);
        RuleFor(command => command.Name).NotEmpty().MaximumLength(150);
        RuleFor(command => command.Description).NotEmpty().MaximumLength(1000);

        RuleFor(command => command.TitleAr).NotEmpty().MaximumLength(200);
        RuleFor(command => command.NameAr).NotEmpty().MaximumLength(150);
        RuleFor(command => command.DescriptionAr).NotEmpty().MaximumLength(1000);

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
