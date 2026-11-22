using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.CallbackRequests;

namespace Tamreed2.Application.CallbackRequests.Commands.CreateCallbackRequest;

public sealed class CreateCallbackRequestCommandHandler
    : IRequestHandler<CreateCallbackRequestCommand, Guid>
{
    private readonly ICallbackRequestRepository _callbackRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCallbackRequestCommandHandler(
        ICallbackRequestRepository callbackRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _callbackRequestRepository = callbackRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateCallbackRequestCommand request,
        CancellationToken cancellationToken)
    {
        var callbackRequest = CallbackRequest.Create(
            request.Name,
            request.MobileNumber,
            request.ServicesNeeded,
            request.Message);

        await _callbackRequestRepository.AddAsync(
            callbackRequest,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return callbackRequest.Id;
    }
}
