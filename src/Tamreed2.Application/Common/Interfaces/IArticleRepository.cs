using Tamreed2.Domain.Articles;

namespace Tamreed2.Application.Common.Interfaces;

public interface IArticleRepository
{
    Task<Article?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Article?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken);
    Task<Article?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<IReadOnlyList<Article>> GetPublishedAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Article>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Article article, CancellationToken cancellationToken);
    void Delete(Article article);
}
