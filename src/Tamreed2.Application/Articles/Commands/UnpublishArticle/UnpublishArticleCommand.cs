using MediatR;

namespace Tamreed2.Application.Articles.Commands.UnpublishArticle;

public sealed record UnpublishArticleCommand(Guid Id) : IRequest<bool>;
