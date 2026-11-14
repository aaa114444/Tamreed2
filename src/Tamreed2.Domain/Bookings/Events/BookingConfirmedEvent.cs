
using Tamreed2.Domain.Common.Events;

namespace Tamreed2.Domain.Bookings.Events;

public sealed record BookingConfirmedEvent(
    Guid BookingId,
    DateTimeOffset OccurredOnUtc
) : IDomainEvent;
