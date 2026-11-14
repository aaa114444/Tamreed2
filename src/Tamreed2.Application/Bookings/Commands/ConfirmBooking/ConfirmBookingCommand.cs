using MediatR;

namespace Tamreed2.Application.Bookings.Commands.ConfirmBooking;

public sealed record ConfirmBookingCommand(Guid BookingId) : IRequest<Unit>;
