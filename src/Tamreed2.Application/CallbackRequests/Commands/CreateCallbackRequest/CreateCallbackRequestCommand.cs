using MediatR;

namespace Tamreed2.Application.CallbackRequests.Commands.CreateCallbackRequest;

public sealed record CreateCallbackRequestCommand(
    string Name,
    string MobileNumber,
    IReadOnlyList<string> ServicesNeeded,
    string? Message
) : IRequest<Guid>;
