using MediatR;

namespace Tamreed2.Application.Articles.Commands.CreateArticle;

public sealed record CreateArticleCommand(
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
) : IRequest<Guid>;
