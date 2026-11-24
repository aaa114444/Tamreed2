namespace Tamreed2.Contracts.Articles;

public sealed record CreateArticleRequest(
    string Title,
    string TitleAr,
    string Slug,
    string Excerpt,
    string ExcerptAr,
    string Content,
    string ContentAr,
    string? CoverImageUrl,
    string? SeoTitle,
    string? SeoTitleAr,
    string? SeoDescription,
    string? SeoDescriptionAr
);
