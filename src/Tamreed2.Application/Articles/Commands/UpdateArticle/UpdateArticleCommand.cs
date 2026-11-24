using MediatR;

namespace Tamreed2.Application.Articles.Commands.UpdateArticle;

public sealed record UpdateArticleCommand(
    Guid Id,
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
) : IRequest<bool>;
