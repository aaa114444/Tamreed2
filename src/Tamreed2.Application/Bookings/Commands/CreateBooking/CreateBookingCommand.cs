using MediatR;

namespace Tamreed2.Application.Bookings.Commands.CreateBooking;

public sealed record CreateBookingCommand(
    Guid ServiceId,
    DateTimeOffset ScheduledAtUtc,
    string City,
    string District,
    string Street,
    string BuildingNumber,
    string? ApartmentNumber,
    string? Notes
) : IRequest<Guid>;
