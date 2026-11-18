using Tamreed2.Domain.Bookings;
using Tamreed2.Domain.Bookings.Events;
using Tamreed2.Domain.Bookings.ValueObjects;
using Tamreed2.Domain.Common.Exceptions;
using Xunit;

namespace Tamreed2.UnitTests.Domain.Bookings;

public class BookingTests
{
    private static Address CreateAddress() =>
        new("Cairo", "Nasr City", "Street 1", "15");

    private static Booking CreateTestBooking() =>
        Booking.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTimeOffset.UtcNow.AddDays(2),
            CreateAddress());

    [Fact]
    public void Create_WithValidData_ShouldCreatePendingBooking()
    {
        var customerId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var scheduledAt = DateTimeOffset.UtcNow.AddDays(2);
        var address = CreateAddress();

        var booking = Booking.Create(
            customerId, serviceId, scheduledAt, address,
            "Please call before arrival.");

        Assert.NotEqual(Guid.Empty, booking.Id);
        Assert.Equal(customerId, booking.CustomerId);
        Assert.Equal(serviceId, booking.ServiceId);
        Assert.Equal(scheduledAt, booking.ScheduledAtUtc);
        Assert.Equal(address, booking.Address);
        Assert.Equal("Please call before arrival.", booking.Notes);
        Assert.Equal(BookingStatus.Pending, booking.Status);
    }

    [Fact]
    public void Create_WithEmptyCustomerId_ShouldThrowDomainException() =>
        Assert.Throws<DomainException>(() =>
            Booking.Create(Guid.Empty, Guid.NewGuid(),
                DateTimeOffset.UtcNow.AddDays(1), CreateAddress()));

    [Fact]
    public void Create_WithEmptyServiceId_ShouldThrowDomainException() =>
        Assert.Throws<DomainException>(() =>
            Booking.Create(Guid.NewGuid(), Guid.Empty,
                DateTimeOffset.UtcNow.AddDays(1), CreateAddress()));

    [Fact]
    public void Create_WithNonUtcDate_ShouldThrowDomainException()
    {
        var date = new DateTimeOffset(
            2026, 12, 1, 10, 0, 0, TimeSpan.FromHours(2));

        Assert.Throws<DomainException>(() =>
            Booking.Create(Guid.NewGuid(), Guid.NewGuid(),
                date, CreateAddress()));
    }

    [Fact]
    public void Create_WithNullAddress_ShouldThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() =>
            Booking.Create(Guid.NewGuid(), Guid.NewGuid(),
                DateTimeOffset.UtcNow.AddDays(1), null!));

    [Fact]
    public void Create_ShouldRaiseBookingCreatedEvent()
    {
        var customerId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        var booking = Booking.Create(
            customerId, serviceId,
            DateTimeOffset.UtcNow.AddDays(1), CreateAddress());

        var createdEvent =
            Assert.IsType<BookingCreatedEvent>(
                Assert.Single(booking.DomainEvents));

        Assert.Equal(booking.Id, createdEvent.BookingId);
        Assert.Equal(customerId, createdEvent.CustomerId);
        Assert.Equal(serviceId, createdEvent.ServiceId);
        Assert.Equal(booking.CreatedAtUtc, createdEvent.OccurredOnUtc);
    }

    [Fact]
    public void Create_WithBlankNotes_ShouldStoreNull()
    {
        var booking = Booking.Create(
            Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.UtcNow.AddDays(1), CreateAddress(), "   ");

        Assert.Null(booking.Notes);
    }

    [Fact]
    public void Confirm_WhenPending_ShouldChangeStatus()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        Assert.Equal(BookingStatus.Confirmed, booking.Status);
    }

    [Fact]
    public void Confirm_WhenAlreadyConfirmed_ShouldThrow()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        Assert.Throws<DomainException>(() => booking.Confirm());
    }

    [Fact]
    public void Confirm_ShouldRaiseEvent()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        Assert.Contains(booking.DomainEvents,
            e => e is BookingConfirmedEvent);
    }

    [Fact]
    public void StartService_WhenConfirmed_ShouldStartService()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        booking.StartService();
        Assert.Equal(BookingStatus.InProgress, booking.Status);
    }

    [Fact]
    public void StartService_WhenPending_ShouldThrow()
    {
        var booking = CreateTestBooking();
        Assert.Throws<DomainException>(() => booking.StartService());
    }

    [Fact]
    public void StartService_ShouldRaiseEvent()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        booking.StartService();
        Assert.Contains(booking.DomainEvents,
            e => e is BookingStartedEvent);
    }

    [Fact]
    public void CompleteService_WhenInProgress_ShouldComplete()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        booking.StartService();
        booking.CompleteService();
        Assert.Equal(BookingStatus.Completed, booking.Status);
    }

    [Fact]
    public void CompleteService_WhenPending_ShouldThrow()
    {
        var booking = CreateTestBooking();
        Assert.Throws<DomainException>(() => booking.CompleteService());
    }

    [Fact]
    public void CompleteService_ShouldRaiseEvent()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        booking.StartService();
        booking.CompleteService();
        Assert.Contains(booking.DomainEvents,
            e => e is BookingCompletedEvent);
    }

    [Theory]
    [InlineData(BookingStatus.Pending)]
    [InlineData(BookingStatus.Confirmed)]
    public void Cancel_WhenNotStarted_ShouldCancel(BookingStatus status)
    {
        var booking = CreateTestBooking();
        if (status == BookingStatus.Confirmed)
            booking.Confirm();

        booking.Cancel();
        Assert.Equal(BookingStatus.Cancelled, booking.Status);
    }

    [Fact]
    public void Cancel_WhenInProgress_ShouldThrow()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        booking.StartService();
        Assert.Throws<DomainException>(() => booking.Cancel());
    }

    [Fact]
    public void Cancel_WhenCompleted_ShouldThrow()
    {
        var booking = CreateTestBooking();
        booking.Confirm();
        booking.StartService();
        booking.CompleteService();
        Assert.Throws<DomainException>(() => booking.Cancel());
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_ShouldThrow()
    {
        var booking = CreateTestBooking();
        booking.Cancel();
        Assert.Throws<DomainException>(() => booking.Cancel());
    }

    [Fact]
    public void Cancel_ShouldRaiseEvent()
    {
        var booking = CreateTestBooking();
        booking.Cancel();
        Assert.Contains(booking.DomainEvents,
            e => e is BookingCancelledEvent);
    }
}
