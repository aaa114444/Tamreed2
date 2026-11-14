using MediatR;
using Tamreed2.Contracts.Bookings;

namespace Tamreed2.Application.Bookings.Queries.GetMyBookings;

public sealed record GetMyBookingsQuery
    : IRequest<IReadOnlyList<BookingResponse>>;
