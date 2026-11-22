using MediatR;
using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Application.CallbackRequests.Commands.UpdateCallbackRequestStatus;

public sealed class UpdateCallbackRequestStatusCommandHandler
    : IRequestHandler<UpdateCallbackRequestStatusCommand, Guid>
{
    private readonly ICallbackRequestRepository _callbackRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCallbackRequestStatusCommandHandler(
        ICallbackRequestRepository callbackRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _callbackRequestRepository = callbackRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        UpdateCallbackRequestStatusCommand request,
        CancellationToken cancellationToken)
    {
        var callbackRequest =
            await _callbackRequestRepository.GetByIdForUpdateAsync(
                request.RequestId,
                cancellationToken);

        if (callbackRequest is null)
        {
            throw new KeyNotFoundException(
                $"Callback request with ID '{request.RequestId}' was not found.");
        }

        callbackRequest.ChangeStatus(request.Status);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return callbackRequest.Id;
    }
}
