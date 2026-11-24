using FluentValidation;

namespace Tamreed2.Application.Articles.Commands.PublishArticle;

public sealed class PublishArticleCommandValidator
    : AbstractValidator<PublishArticleCommand>
{
    public PublishArticleCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
    }
}
