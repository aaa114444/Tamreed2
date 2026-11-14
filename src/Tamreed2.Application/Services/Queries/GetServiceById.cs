using MediatR;
using Tamreed2.Contracts.Services;

namespace Tamreed2.Application.Services.Queries.GetServiceById;

public sealed record GetServiceByIdQuery(
    Guid ServiceId
) : IRequest<ServiceResponse?>;