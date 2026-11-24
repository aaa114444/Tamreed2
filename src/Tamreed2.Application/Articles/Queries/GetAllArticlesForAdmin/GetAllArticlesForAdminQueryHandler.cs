using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Contracts.Articles;

namespace Tamreed2.Application.Articles.Queries.GetAllArticlesForAdmin;

public sealed class GetAllArticlesForAdminQueryHandler
    : IRequestHandler<GetAllArticlesForAdminQuery, IReadOnlyList<ArticleResponse>>
{
    private readonly IArticleRepository _articleRepository;

    public GetAllArticlesForAdminQueryHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<IReadOnlyList<ArticleResponse>> Handle(
        GetAllArticlesForAdminQuery request,
        CancellationToken cancellationToken)
    {
        var articles = await _articleRepository.GetAllAsync(cancellationToken);

        return articles.Select(ArticleResponseMapper.Map).ToList();
    }
}
