using MediatR;
using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Application.Articles.Commands.DeleteArticle;

public sealed class DeleteArticleCommandHandler
    : IRequestHandler<DeleteArticleCommand, bool>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteArticleCommandHandler(
        IArticleRepository articleRepository,
        IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        DeleteArticleCommand request,
        CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdForUpdateAsync(
            request.Id, cancellationToken);

        if (article is null)
            return false;

        _articleRepository.Delete(article);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
