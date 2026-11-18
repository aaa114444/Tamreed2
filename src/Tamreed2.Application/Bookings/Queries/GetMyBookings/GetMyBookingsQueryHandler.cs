using MediatR;
using Tamreed2.Application.Common.Exceptions;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Contracts.Bookings;

namespace Tamreed2.Application.Bookings.Queries.GetMyBookings;

public sealed class GetMyBookingsQueryHandler
    : IRequestHandler<GetMyBookingsQuery, IReadOnlyList<BookingResponse>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICurrentUserContext _currentUserContext;

    public GetMyBookingsQueryHandler(
        IBookingRepository bookingRepository,
        ICurrentUserContext currentUserContext)
    {
        _bookingRepository = bookingRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task<IReadOnlyList<BookingResponse>> Handle(
        GetMyBookingsQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserContext.IsAuthenticated ||
            !_currentUserContext.UserId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "You must be authenticated to view your bookings.");
        }

        if (!string.Equals(
                _currentUserContext.Role,
                "User",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ForbiddenAccessException(
                "Only users can access their personal bookings.");
        }

        var bookings = await _bookingRepository.GetByCustomerIdAsync(
            _currentUserContext.UserId.Value,
            cancellationToken);

        return bookings.Select(booking => new BookingResponse(
            booking.Id,
            booking.CustomerId,
            booking.ServiceId,
            booking.ScheduledAtUtc,
            booking.Address.City,
            booking.Address.District,
            booking.Address.Street,
            booking.Address.BuildingNumber,
            booking.Address.ApartmentNumber,
            booking.Notes,
            booking.Status.ToString(),
            booking.CreatedAtUtc
        )).ToList();
    }
}

