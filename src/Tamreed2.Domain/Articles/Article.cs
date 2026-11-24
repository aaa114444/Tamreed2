using Tamreed2.Domain.Common.Exceptions;
using Tamreed2.Domain.Common.Models;

namespace Tamreed2.Domain.Articles;

public sealed class Article : AggregateRoot
{
    public string Title { get; private set; } = null!;
    public string TitleAr { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string Excerpt { get; private set; } = null!;
    public string ExcerptAr { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public string ContentAr { get; private set; } = null!;
    public string CoverImageUrl { get; private set; } = string.Empty;
    public string SeoTitle { get; private set; } = string.Empty;
    public string SeoTitleAr { get; private set; } = string.Empty;
    public string SeoDescription { get; private set; } = string.Empty;
    public string SeoDescriptionAr { get; private set; } = string.Empty;
    public ArticleStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }
    public DateTimeOffset? PublishedAtUtc { get; private set; }

    private Article() { }

    private Article(
        string title, string titleAr, string slug,
        string excerpt, string excerptAr,
        string content, string contentAr,
        string? coverImageUrl,
        string? seoTitle, string? seoTitleAr,
        string? seoDescription, string? seoDescriptionAr)
    {
        Update(
            title, titleAr, slug, excerpt, excerptAr,
            content, contentAr, coverImageUrl,
            seoTitle, seoTitleAr, seoDescription, seoDescriptionAr);

        Status = ArticleStatus.Draft;
        CreatedAtUtc = DateTimeOffset.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
    }

    public static Article Create(
        string title, string titleAr, string slug,
        string excerpt, string excerptAr,
        string content, string contentAr,
        string? coverImageUrl = null,
        string? seoTitle = null, string? seoTitleAr = null,
        string? seoDescription = null, string? seoDescriptionAr = null)
    {
        return new Article(
            title, titleAr, slug, excerpt, excerptAr,
            content, contentAr, coverImageUrl,
            seoTitle, seoTitleAr, seoDescription, seoDescriptionAr);
    }

    public void Update(
        string title, string titleAr, string slug,
        string excerpt, string excerptAr,
        string content, string contentAr,
        string? coverImageUrl,
        string? seoTitle, string? seoTitleAr,
        string? seoDescription, string? seoDescriptionAr)
    {
        if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(titleAr))
            throw new DomainException("Article title is required in at least one language.");

        if (string.IsNullOrWhiteSpace(content) && string.IsNullOrWhiteSpace(contentAr))
            throw new DomainException("Article content is required in at least one language.");

        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException("Article slug is required.");

        slug = slug.Trim().ToLowerInvariant();

        if (slug.Length > 180)
            throw new DomainException("Article slug cannot exceed 180 characters.");

        if (slug.Any(c => !char.IsLetterOrDigit(c) && c != '-'))
            throw new DomainException("Article slug can contain only letters, numbers, and hyphens.");

        Title = (title ?? string.Empty).Trim();
        TitleAr = (titleAr ?? string.Empty).Trim();
        Slug = slug;
        Excerpt = (excerpt ?? string.Empty).Trim();
        ExcerptAr = (excerptAr ?? string.Empty).Trim();
        Content = content ?? string.Empty;
        ContentAr = contentAr ?? string.Empty;
        CoverImageUrl = (coverImageUrl ?? string.Empty).Trim();
        SeoTitle = (seoTitle ?? string.Empty).Trim();
        SeoTitleAr = (seoTitleAr ?? string.Empty).Trim();
        SeoDescription = (seoDescription ?? string.Empty).Trim();
        SeoDescriptionAr = (seoDescriptionAr ?? string.Empty).Trim();
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Publish()
    {
        if (Status == ArticleStatus.Published)
            return;

        Status = ArticleStatus.Published;
        PublishedAtUtc = DateTimeOffset.UtcNow;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Unpublish()
    {
        Status = ArticleStatus.Draft;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }
}
