using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly Tamreed2DbContext _dbContext;

    public UnitOfWork(Tamreed2DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}