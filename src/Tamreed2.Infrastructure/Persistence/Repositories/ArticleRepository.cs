using Microsoft.EntityFrameworkCore;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Articles;

namespace Tamreed2.Infrastructure.Persistence.Repositories;

public sealed class ArticleRepository : IArticleRepository
{
    private readonly Tamreed2DbContext _dbContext;

    public ArticleRepository(Tamreed2DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Article?> GetByIdAsync(
        Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(article => article.Id == id, cancellationToken);
    }

    public async Task<Article?> GetByIdForUpdateAsync(
        Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Articles
            .FirstOrDefaultAsync(article => article.Id == id, cancellationToken);
    }

    public async Task<Article?> GetBySlugAsync(
        string slug, CancellationToken cancellationToken)
    {
        return await _dbContext.Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(article => article.Slug == slug, cancellationToken);
    }

    public async Task<IReadOnlyList<Article>> GetPublishedAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Articles
            .AsNoTracking()
            .Where(article => article.Status == ArticleStatus.Published)
            .OrderByDescending(article => article.PublishedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Article>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Articles
            .AsNoTracking()
            .OrderByDescending(article => article.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public void Delete(Article article)
    {
        _dbContext.Articles.Remove(article);
    }

    public async Task AddAsync(
        Article article, CancellationToken cancellationToken)
    {
        await _dbContext.Articles.AddAsync(article, cancellationToken);
    }
}
