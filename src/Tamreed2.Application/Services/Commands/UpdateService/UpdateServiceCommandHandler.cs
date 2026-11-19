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
        var service = await _serviceRepository.GetByIdForUpdateAsync(
            request.ServiceId,
            cancellationToken);

        if (service is null)
        {
            throw new KeyNotFoundException(
                $"Service with ID '{request.ServiceId}' was not found.");
        }

        service.UpdateDetails(
            request.Title,
            request.Name,
            request.Description,
            request.Points,
            request.TitleAr,
            request.NameAr,
            request.DescriptionAr,
            request.PointsAr,
            request.Details);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return service.Id;
    }
}

