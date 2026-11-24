using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Contracts.Articles;

namespace Tamreed2.Application.Articles.Queries.GetPublishedArticles;

public sealed class GetPublishedArticlesQueryHandler
    : IRequestHandler<GetPublishedArticlesQuery, IReadOnlyList<ArticleResponse>>
{
    private readonly IArticleRepository _articleRepository;

    public GetPublishedArticlesQueryHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<IReadOnlyList<ArticleResponse>> Handle(
        GetPublishedArticlesQuery request,
        CancellationToken cancellationToken)
    {
        var articles = await _articleRepository.GetPublishedAsync(cancellationToken);

        return articles.Select(ArticleResponseMapper.Map).ToList();
    }
}
