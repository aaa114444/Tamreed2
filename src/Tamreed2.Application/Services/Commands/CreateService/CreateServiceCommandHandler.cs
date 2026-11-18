using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Services;

namespace Tamreed2.Application.Services.Commands.CreateService;

public sealed class CreateServiceCommandHandler
    : IRequestHandler<CreateServiceCommand, Guid>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceCommandHandler(
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateServiceCommand request,
        CancellationToken cancellationToken)
    {
        var service = Service.Create(
            request.Title,
            request.Name,
            request.Description,
            request.Points,
            request.TitleAr,
            request.NameAr,
            request.DescriptionAr,
            request.PointsAr);

        await _serviceRepository.AddAsync(service, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return service.Id;
    }
}
