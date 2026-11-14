using MediatR;
using Tamreed2.Contracts.Bookings;

namespace Tamreed2.Application.Bookings.Queries.GetAllBookings;

public sealed record GetAllBookingsQuery
    : IRequest<IReadOnlyList<BookingResponse>>;
