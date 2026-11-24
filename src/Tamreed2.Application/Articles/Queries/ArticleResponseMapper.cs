using Tamreed2.Contracts.Articles;
using Tamreed2.Domain.Articles;

namespace Tamreed2.Application.Articles.Queries;

internal static class ArticleResponseMapper
{
    public static ArticleResponse Map(Article article)
    {
        return new ArticleResponse(
            article.Id,
            article.Title,
            article.TitleAr,
            article.Slug,
            article.Excerpt,
            article.ExcerptAr,
            article.Content,
            article.ContentAr,
            article.CoverImageUrl,
            article.SeoTitle,
            article.SeoTitleAr,
            article.SeoDescription,
            article.SeoDescriptionAr,
            article.Status.ToString(),
            article.CreatedAtUtc,
            article.UpdatedAtUtc,
            article.PublishedAtUtc);
    }
}
