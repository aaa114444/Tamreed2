using MediatR;

namespace Tamreed2.Application.Articles.Commands.PublishArticle;

public sealed record PublishArticleCommand(Guid Id) : IRequest<bool>;
