
using Microsoft.EntityFrameworkCore;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Services;

namespace Tamreed2.Infrastructure.Persistence.Repositories;

public sealed class ServiceRepository : IServiceRepository
{
    private readonly Tamreed2DbContext _dbContext;

    public ServiceRepository(Tamreed2DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Used for read-only operations.
    public async Task<Service?> GetByIdAsync(
        Guid serviceId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(
                service => service.Id == serviceId,
                cancellationToken);
    }

    // Used when we intend to modify the service.
    // EF Core tracks the returned entity.
    public async Task<Service?> GetByIdForUpdateAsync(
        Guid serviceId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .FirstOrDefaultAsync(
                service => service.Id == serviceId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Service>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .AsNoTracking()
            .OrderBy(service => service.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Service service,
        CancellationToken cancellationToken)
    {
        await _dbContext.Services.AddAsync(
            service,
            cancellationToken);
    }
}
