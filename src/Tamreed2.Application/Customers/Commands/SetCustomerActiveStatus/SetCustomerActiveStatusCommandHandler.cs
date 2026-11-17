using MediatR;
using Tamreed2.Application.Common.Exceptions;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Users;

namespace Tamreed2.Application.Customers.Commands.SetCustomerActiveStatus;

public sealed class SetCustomerActiveStatusCommandHandler
    : IRequestHandler<SetCustomerActiveStatusCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetCustomerActiveStatusCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        SetCustomerActiveStatusCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _userRepository.GetByIdForUpdateAsync(
            request.CustomerId,
            cancellationToken);

        if (customer is null)
            throw new KeyNotFoundException("Customer was not found.");

        if (customer.Role != UserRole.User)
            throw new ForbiddenAccessException(
                "Only customer accounts can be activated or deactivated.");

        if (customer.IsActive != request.IsActive)
        {
            if (request.IsActive)
                customer.Activate();
            else
                customer.Deactivate();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
