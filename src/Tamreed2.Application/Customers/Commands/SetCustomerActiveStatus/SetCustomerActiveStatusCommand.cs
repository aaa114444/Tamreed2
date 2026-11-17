using MediatR;

namespace Tamreed2.Application.Customers.Commands.SetCustomerActiveStatus;

public sealed record SetCustomerActiveStatusCommand(
    Guid CustomerId,
    bool IsActive) : IRequest<Unit>;
