
namespace Tamreed2.Domain.Common.Events;

public interface IDomainEvent
{
    DateTimeOffset OccurredOnUtc { get; }
}
