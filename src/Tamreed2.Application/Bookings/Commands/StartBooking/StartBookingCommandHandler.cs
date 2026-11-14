using MediatR;
using Tamreed2.Application.Common.Exceptions;
using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Application.Bookings.Commands.StartBooking;

public sealed class StartBookingCommandHandler
    : IRequestHandler<StartBookingCommand, Unit>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserContext _currentUserContext;

    public StartBookingCommandHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserContext currentUserContext)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _currentUserContext = currentUserContext;
    }

    public async Task<Unit> Handle(
        StartBookingCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserContext.IsAuthenticated)
            throw new UnauthorizedAccessException("Authentication is required.");

        if (!string.Equals(_currentUserContext.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            throw new ForbiddenAccessException("Only administrators can start bookings.");

        var booking = await _bookingRepository.GetByIdForUpdateAsync(
            request.BookingId, cancellationToken);

        if (booking is null)
            throw new KeyNotFoundException("Booking was not found.");

        booking.StartService();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
