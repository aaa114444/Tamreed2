
using Tamreed2.Domain.Common.Events;
using Tamreed2.Domain.Common.Models;

namespace Tamreed2.UnitTests.Domain.Common.Models;

public class AggregateRootTests
{
    [Fact]
    public void DomainEvents_WhenCreated_ShouldBeEmpty()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();

        // Assert
        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void AddDomainEvent_WhenCalled_ShouldAddEvent()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        var domainEvent = new TestDomainEvent();

        // Act
        aggregate.AddTestDomainEvent(domainEvent);

        // Assert
        Assert.Single(aggregate.DomainEvents);
        Assert.Contains(domainEvent, aggregate.DomainEvents);
    }

    [Fact]
    public void AddDomainEvent_WhenEventIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => aggregate.AddTestDomainEvent(null!));
    }

    [Fact]
    public void ClearDomainEvents_WhenEventsExist_ShouldRemoveAllEvents()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();

        aggregate.AddTestDomainEvent(new TestDomainEvent());
        aggregate.AddTestDomainEvent(new TestDomainEvent());

        // Act
        aggregate.ClearDomainEvents();

        // Assert
        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void DomainEvents_ShouldNotAllowExternalModification()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        var domainEvent = new TestDomainEvent();

        aggregate.AddTestDomainEvent(domainEvent);

        // Act
        var events = aggregate.DomainEvents;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<IDomainEvent>>(events);
        Assert.Single(events);
    }

    // Concrete implementation for testing the abstract AggregateRoot.
    private sealed class TestAggregateRoot : AggregateRoot
    {
        public void AddTestDomainEvent(IDomainEvent domainEvent)
        {
            AddDomainEvent(domainEvent);
        }
    }

    // Fake event used only in unit tests.
    private sealed class TestDomainEvent : IDomainEvent
    {
        public DateTimeOffset OccurredOnUtc { get; } =
            DateTimeOffset.UtcNow;
    }
}
