using MediatR;
using Tamreed2.Application.Common.Exceptions;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Contracts.Bookings;

namespace Tamreed2.Application.Bookings.Queries.GetAllBookings;

public sealed class GetAllBookingsQueryHandler
    : IRequestHandler<GetAllBookingsQuery, IReadOnlyList<BookingResponse>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICurrentUserContext _currentUserContext;

    public GetAllBookingsQueryHandler(
        IBookingRepository bookingRepository,
        ICurrentUserContext currentUserContext)
    {
        _bookingRepository = bookingRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task<IReadOnlyList<BookingResponse>> Handle(
        GetAllBookingsQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserContext.IsAuthenticated)
        {
            throw new UnauthorizedAccessException(
                "You must be authenticated to view bookings.");
        }

        if (!string.Equals(
                _currentUserContext.Role,
                "Admin",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ForbiddenAccessException(
                "Only administrators can view all bookings.");
        }

        var bookings = await _bookingRepository.GetAllAsync(
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

