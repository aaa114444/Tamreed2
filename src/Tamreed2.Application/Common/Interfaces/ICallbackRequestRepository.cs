using Tamreed2.Domain.CallbackRequests;

namespace Tamreed2.Application.Common.Interfaces;

public interface ICallbackRequestRepository
{
    Task AddAsync(CallbackRequest request, CancellationToken cancellationToken);

    Task<CallbackRequest?> GetByIdForUpdateAsync(
        Guid requestId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<CallbackRequest>> GetAllAsync(
        CancellationToken cancellationToken);
}