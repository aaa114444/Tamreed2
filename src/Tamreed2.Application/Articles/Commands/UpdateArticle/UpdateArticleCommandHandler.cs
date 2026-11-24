using MediatR;
using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Application.Articles.Commands.UpdateArticle;

public sealed class UpdateArticleCommandHandler
    : IRequestHandler<UpdateArticleCommand, bool>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateArticleCommandHandler(
        IArticleRepository articleRepository,
        IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        UpdateArticleCommand request,
        CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdForUpdateAsync(
            request.Id, cancellationToken);

        if (article is null)
            return false;

        var slug = request.Slug.Trim().ToLowerInvariant();
        var existingArticle = await _articleRepository.GetBySlugAsync(
            slug, cancellationToken);

        if (existingArticle is not null && existingArticle.Id != request.Id)
            throw new InvalidOperationException("An article with this slug already exists.");

        article.Update(
            request.Title,
            request.TitleAr,
            slug,
            request.Excerpt,
            request.ExcerptAr,
            request.Content,
            request.ContentAr,
            request.CoverImageUrl,
            request.SeoTitle,
            request.SeoTitleAr,
            request.SeoDescription,
            request.SeoDescriptionAr);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
