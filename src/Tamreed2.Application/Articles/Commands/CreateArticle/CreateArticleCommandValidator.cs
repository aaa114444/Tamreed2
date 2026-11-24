using FluentValidation;

namespace Tamreed2.Application.Articles.Commands.CreateArticle;

public sealed class CreateArticleCommandValidator
    : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(command => command)
            .Must(command =>
                !string.IsNullOrWhiteSpace(command.Title) ||
                !string.IsNullOrWhiteSpace(command.TitleAr))
            .WithMessage("Article title is required in at least one language.");

        RuleFor(command => command.Title).MaximumLength(200);
        RuleFor(command => command.TitleAr).MaximumLength(200);

        RuleFor(command => command.Slug)
            .NotEmpty()
            .MaximumLength(180)
            .Matches("^[a-zA-Z0-9-]+$")
            .WithMessage("Slug can contain only letters, numbers, and hyphens.");

        RuleFor(command => command)
            .Must(command =>
                !string.IsNullOrWhiteSpace(command.Content) ||
                !string.IsNullOrWhiteSpace(command.ContentAr))
            .WithMessage("Article content is required in at least one language.");

        RuleFor(command => command.Excerpt).MaximumLength(1000);
        RuleFor(command => command.ExcerptAr).MaximumLength(1000);
        RuleFor(command => command.CoverImageUrl).MaximumLength(1000);
        RuleFor(command => command.SeoTitle).MaximumLength(200);
        RuleFor(command => command.SeoTitleAr).MaximumLength(200);
        RuleFor(command => command.SeoDescription).MaximumLength(500);
        RuleFor(command => command.SeoDescriptionAr).MaximumLength(500);
    }
}
