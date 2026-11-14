namespace Tamreed2.Contracts.Bookings;

public sealed record CreateBookingRequest(
    Guid ServiceId,
    DateTimeOffset ScheduledAtUtc,
    string City,
    string District,
    string Street,
    string BuildingNumber,
    string? ApartmentNumber,
    string? Notes
);
