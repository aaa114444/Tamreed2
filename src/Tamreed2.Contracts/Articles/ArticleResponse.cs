namespace Tamreed2.Contracts.Articles;

public sealed record ArticleResponse(
    Guid Id,
    string Title,
    string TitleAr,
    string Slug,
    string Excerpt,
    string ExcerptAr,
    string Content,
    string ContentAr,
    string CoverImageUrl,
    string SeoTitle,
    string SeoTitleAr,
    string SeoDescription,
    string SeoDescriptionAr,
    string Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc,
    DateTimeOffset? PublishedAtUtc
);
