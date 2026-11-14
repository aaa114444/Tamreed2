using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Contracts.Services;

namespace Tamreed2.Application.Services.Queries.GetServiceById;

public sealed class GetServiceByIdQueryHandler
    : IRequestHandler<GetServiceByIdQuery, ServiceResponse?>
{
    private readonly IServiceRepository _serviceRepository;

    public GetServiceByIdQueryHandler(
        IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ServiceResponse?> Handle(
        GetServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(
            request.ServiceId,
            cancellationToken);

        if (service is null)
        {
            return null;
        }

        return new ServiceResponse(
            service.Id,
            service.Name,
            service.Description,
            service.Price,
            service.Status.ToString(),
            service.CreatedAtUtc);
    }
}