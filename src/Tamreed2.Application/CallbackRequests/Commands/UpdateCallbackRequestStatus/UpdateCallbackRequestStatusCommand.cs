using MediatR;
using Tamreed2.Domain.CallbackRequests;

namespace Tamreed2.Application.CallbackRequests.Commands.UpdateCallbackRequestStatus;

public sealed record UpdateCallbackRequestStatusCommand(
    Guid RequestId,
    CallbackRequestStatus Status
) : IRequest<Guid>;
