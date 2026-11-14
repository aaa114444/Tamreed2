
using MediatR;

namespace Tamreed2.Application.Services.Commands.UpdateService;

public sealed record UpdateServiceCommand(
    Guid ServiceId,
    string Name,
    string Description,
    decimal Price
) : IRequest<Guid>;
