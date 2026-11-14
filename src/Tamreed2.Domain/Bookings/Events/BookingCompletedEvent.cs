
using Tamreed2.Domain.Common.Events;

namespace Tamreed2.Domain.Bookings.Events;

public sealed record BookingCompletedEvent(
    Guid BookingId,
    DateTimeOffset OccurredOnUtc
) : IDomainEvent;
