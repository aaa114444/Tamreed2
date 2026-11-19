using MediatR;
using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Application.Services.Commands.DeleteService;

public sealed class DeleteServiceCommandHandler
    : IRequestHandler<DeleteServiceCommand, Unit>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceCommandHandler(
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteServiceCommand request,
        CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdForUpdateAsync(
            request.ServiceId,
            cancellationToken);

        if (service is null)
            throw new KeyNotFoundException("Service was not found.");

        service.Deactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
