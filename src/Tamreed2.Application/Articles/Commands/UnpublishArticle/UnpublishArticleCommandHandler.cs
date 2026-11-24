using MediatR;
using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Application.Articles.Commands.UnpublishArticle;

public sealed class UnpublishArticleCommandHandler
    : IRequestHandler<UnpublishArticleCommand, bool>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnpublishArticleCommandHandler(
        IArticleRepository articleRepository,
        IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        UnpublishArticleCommand request,
        CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdForUpdateAsync(
            request.Id, cancellationToken);

        if (article is null)
            return false;

        article.Unpublish();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
