
using MediatR;
using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Application.Services.Commands.UpdateService;

public sealed class UpdateServiceCommandHandler
    : IRequestHandler<UpdateServiceCommand, Guid>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceCommandHandler(
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        UpdateServiceCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Retrieve the service as a tracked entity.
        var service = await _serviceRepository.GetByIdForUpdateAsync(
            request.ServiceId,
            cancellationToken);

        // 2. Ensure that the service exists.
        if (service is null)
        {
            throw new KeyNotFoundException(
                $"Service with ID '{request.ServiceId}' was not found.");
        }

        // 3. Apply the update through the domain entity.
        service.UpdateDetails(
            request.Name,
            request.Description,
            request.Price);

        // 4. Persist the changes.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 5. Return the updated service ID.
        return service.Id;
    }
}
