namespace Tamreed2.Contracts.Bookings;

public sealed record BookingResponse(
    Guid Id,
    Guid CustomerId,
    Guid ServiceId,
    DateTimeOffset ScheduledAtUtc,
    string City,
    string District,
    string Street,
    string BuildingNumber,
    string? ApartmentNumber,
    string? Notes,
    decimal PriceAtBooking,
    string Status,
    DateTimeOffset CreatedAtUtc
);
