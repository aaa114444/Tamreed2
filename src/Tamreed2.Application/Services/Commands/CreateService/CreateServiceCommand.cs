using MediatR;

namespace Tamreed2.Application.Services.Commands.CreateService;

public sealed record CreateServiceCommand(
    string Name,
    string Description,
    decimal Price
) : IRequest<Guid>;