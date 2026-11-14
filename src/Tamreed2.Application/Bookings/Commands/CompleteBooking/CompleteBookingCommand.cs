using MediatR;

namespace Tamreed2.Application.Bookings.Commands.CompleteBooking;

public sealed record CompleteBookingCommand(Guid BookingId) : IRequest<Unit>;
