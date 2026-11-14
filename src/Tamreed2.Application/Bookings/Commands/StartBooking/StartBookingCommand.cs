using MediatR;

namespace Tamreed2.Application.Bookings.Commands.StartBooking;

public sealed record StartBookingCommand(Guid BookingId) : IRequest<Unit>;
