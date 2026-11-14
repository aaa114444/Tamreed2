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
        // 1. Create the Service domain entity.
        var service = Service.Create(
            request.Name,
            request.Description,
            request.Price);

        // 2. Add it to the repository.
        await _serviceRepository.AddAsync(
            service,
            cancellationToken);

        // 3. Save changes to the database.
        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 4. Return the generated service ID.
        return service.Id;
    }
}