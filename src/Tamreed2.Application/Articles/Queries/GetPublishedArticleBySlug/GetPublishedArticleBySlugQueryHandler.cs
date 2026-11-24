using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Contracts.Articles;
using Tamreed2.Domain.Articles;
using Tamreed2.Application.Articles.Queries;

namespace Tamreed2.Application.Articles.Queries.GetPublishedArticleBySlug;

public sealed class GetPublishedArticleBySlugQueryHandler
    : IRequestHandler<GetPublishedArticleBySlugQuery, ArticleResponse?>
{
    private readonly IArticleRepository _articleRepository;

    public GetPublishedArticleBySlugQueryHandler(
        IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<ArticleResponse?> Handle(
        GetPublishedArticleBySlugQuery request,
        CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetBySlugAsync(
            request.Slug.Trim().ToLowerInvariant(),
            cancellationToken);

        if (article is null || article.Status != ArticleStatus.Published)
            return null;

        return ArticleResponseMapper.Map(article);
    }
}
