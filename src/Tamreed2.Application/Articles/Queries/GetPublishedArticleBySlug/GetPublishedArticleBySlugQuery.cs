using MediatR;
using Tamreed2.Contracts.Articles;

namespace Tamreed2.Application.Articles.Queries.GetPublishedArticleBySlug;

public sealed record GetPublishedArticleBySlugQuery(string Slug)
    : IRequest<ArticleResponse?>;
