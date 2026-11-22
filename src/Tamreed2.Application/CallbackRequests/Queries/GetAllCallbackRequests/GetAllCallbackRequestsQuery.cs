using MediatR;
using Tamreed2.Contracts.CallbackRequests;

namespace Tamreed2.Application.CallbackRequests.Queries.GetAllCallbackRequests;

public sealed record GetAllCallbackRequestsQuery
    : IRequest<IReadOnlyList<CallbackRequestResponse>>;
