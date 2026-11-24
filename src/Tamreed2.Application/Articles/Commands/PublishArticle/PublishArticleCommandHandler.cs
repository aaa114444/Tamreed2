using MediatR;
using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Application.Articles.Commands.PublishArticle;

public sealed class PublishArticleCommandHandler
    : IRequestHandler<PublishArticleCommand, bool>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublishArticleCommandHandler(
        IArticleRepository articleRepository,
        IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        PublishArticleCommand request,
        CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdForUpdateAsync(
            request.Id, cancellationToken);

        if (article is null)
            return false;

        article.Publish();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
