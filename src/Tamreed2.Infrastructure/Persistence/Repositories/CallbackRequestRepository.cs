using Microsoft.EntityFrameworkCore;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.CallbackRequests;

namespace Tamreed2.Infrastructure.Persistence.Repositories;

public sealed class CallbackRequestRepository : ICallbackRequestRepository
{
    private readonly Tamreed2DbContext _dbContext;

    public CallbackRequestRepository(Tamreed2DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        CallbackRequest request,
        CancellationToken cancellationToken)
    {
        await _dbContext.CallbackRequests.AddAsync(request, cancellationToken);
    }

    public async Task<CallbackRequest?> GetByIdForUpdateAsync(
        Guid requestId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.CallbackRequests
            .FirstOrDefaultAsync(
                request => request.Id == requestId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<CallbackRequest>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbContext.CallbackRequests
            .AsNoTracking()
            .OrderByDescending(request => request.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}