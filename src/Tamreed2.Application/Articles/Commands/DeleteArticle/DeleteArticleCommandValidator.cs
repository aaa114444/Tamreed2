using FluentValidation;

namespace Tamreed2.Application.Articles.Commands.DeleteArticle;

public sealed class DeleteArticleCommandValidator
    : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
    }
}
