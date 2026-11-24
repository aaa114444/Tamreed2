using FluentValidation;

namespace Tamreed2.Application.Articles.Commands.UnpublishArticle;

public sealed class UnpublishArticleCommandValidator
    : AbstractValidator<UnpublishArticleCommand>
{
    public UnpublishArticleCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
    }
}
