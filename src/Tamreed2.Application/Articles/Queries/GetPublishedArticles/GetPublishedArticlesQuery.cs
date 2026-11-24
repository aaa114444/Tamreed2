using MediatR;
using Tamreed2.Contracts.Articles;

namespace Tamreed2.Application.Articles.Queries.GetPublishedArticles;

public sealed record GetPublishedArticlesQuery
    : IRequest<IReadOnlyList<ArticleResponse>>;
