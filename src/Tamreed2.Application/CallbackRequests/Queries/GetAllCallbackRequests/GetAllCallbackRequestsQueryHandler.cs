using MediatR;
using Tamreed2.Application.Common.Exceptions;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Contracts.CallbackRequests;

namespace Tamreed2.Application.CallbackRequests.Queries.GetAllCallbackRequests;

public sealed class GetAllCallbackRequestsQueryHandler
    : IRequestHandler<GetAllCallbackRequestsQuery, IReadOnlyList<CallbackRequestResponse>>
{
    private readonly ICallbackRequestRepository _callbackRequestRepository;
    private readonly ICurrentUserContext _currentUserContext;

    public GetAllCallbackRequestsQueryHandler(
        ICallbackRequestRepository callbackRequestRepository,
        ICurrentUserContext currentUserContext)
    {
        _callbackRequestRepository = callbackRequestRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task<IReadOnlyList<CallbackRequestResponse>> Handle(
        GetAllCallbackRequestsQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserContext.IsAuthenticated)
        {
            throw new UnauthorizedAccessException(
                "You must be authenticated to view callback requests.");
        }

        if (!string.Equals(
                _currentUserContext.Role,
                "Admin",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ForbiddenAccessException(
                "Only administrators can view callback requests.");
        }

        var requests = await _callbackRequestRepository.GetAllAsync(
            cancellationToken);

        return requests.Select(item => new CallbackRequestResponse(
            item.Id,
            item.Name,
            item.MobileNumber,
            item.ServicesNeeded,
            item.Message,
            item.Status.ToString(),
            item.CreatedAtUtc,
            item.UpdatedAtUtc
        )).ToList();
    }
}
