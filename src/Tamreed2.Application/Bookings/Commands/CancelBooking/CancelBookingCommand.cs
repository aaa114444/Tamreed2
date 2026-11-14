using MediatR;

namespace Tamreed2.Application.Bookings.Commands.CancelBooking;

public sealed record CancelBookingCommand(Guid BookingId) : IRequest<Unit>;
