
using Tamreed2.Domain.Common.Events;

namespace Tamreed2.Domain.Bookings.Events;

public sealed record BookingCreatedEvent(
    Guid BookingId,
    Guid CustomerId,
    Guid ServiceId,
    DateTimeOffset OccurredOnUtc
) : IDomainEvent;
