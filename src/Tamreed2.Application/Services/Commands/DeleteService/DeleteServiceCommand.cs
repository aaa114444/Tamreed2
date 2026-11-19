using MediatR;

namespace Tamreed2.Application.Services.Commands.DeleteService;

public sealed record DeleteServiceCommand(Guid ServiceId) : IRequest<Unit>;
