using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Articles;

namespace Tamreed2.Application.Articles.Commands.CreateArticle;

public sealed class CreateArticleCommandHandler
    : IRequestHandler<CreateArticleCommand, Guid>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateArticleCommandHandler(
        IArticleRepository articleRepository,
        IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateArticleCommand request,
        CancellationToken cancellationToken)
    {
        var existingArticle = await _articleRepository.GetBySlugAsync(
            request.Slug.Trim().ToLowerInvariant(),
            cancellationToken);

        if (existingArticle is not null)
            throw new InvalidOperationException("An article with this slug already exists.");

        var article = Article.Create(
            request.Title,
            request.TitleAr,
            request.Slug,
            request.Excerpt,
            request.ExcerptAr,
            request.Content,
            request.ContentAr,
            request.CoverImageUrl,
            request.SeoTitle,
            request.SeoTitleAr,
            request.SeoDescription,
            request.SeoDescriptionAr);

        await _articleRepository.AddAsync(article, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return article.Id;
    }
}
