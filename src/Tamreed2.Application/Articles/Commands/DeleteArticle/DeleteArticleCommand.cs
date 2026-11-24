using MediatR;

namespace Tamreed2.Application.Articles.Commands.DeleteArticle;

public sealed record DeleteArticleCommand(Guid Id) : IRequest<bool>;
