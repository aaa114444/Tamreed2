using Tamreed2.Domain.Services;

namespace Tamreed2.Application.Common.Interfaces;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(
        Guid serviceId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Service>> GetAllAsync(
        CancellationToken cancellationToken);

    Task AddAsync(
        Service service,
        CancellationToken cancellationToken);

    Task<Service?> GetByIdForUpdateAsync(
    Guid serviceId,
    CancellationToken cancellationToken);
}