using MediatR;
using Tamreed2.Contracts.Services;

namespace Tamreed2.Application.Services.Queries.GetAllServices;

public sealed record GetAllServicesQuery
    : IRequest<IReadOnlyList<ServiceResponse>>;