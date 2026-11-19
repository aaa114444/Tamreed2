using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Contracts.Services;

namespace Tamreed2.Application.Services.Queries.GetAllServices;

public sealed class GetAllServicesQueryHandler
    : IRequestHandler<GetAllServicesQuery, IReadOnlyList<ServiceResponse>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetAllServicesQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<IReadOnlyList<ServiceResponse>> Handle(
        GetAllServicesQuery request,
        CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetAllAsync(cancellationToken);

        return services.Select(ServiceResponseMapper.Map).ToList();
    }
}
