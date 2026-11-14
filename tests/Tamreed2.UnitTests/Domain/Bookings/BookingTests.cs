
using Tamreed2.Domain.Bookings;
using Tamreed2.Domain.Bookings.Events;
using Tamreed2.Domain.Bookings.ValueObjects;
using Tamreed2.Domain.Common.Exceptions;
using Xunit;

namespace Tamreed2.UnitTests.Domain.Bookings;

public class BookingTests
{
    private static Address CreateAddress()
    {
        return new Address(
            "Cairo",
            "Nasr City",
            "Street 1",
            "15");
    }

    [Fact]
    public void Create_WithValidData_ShouldCreatePendingBooking()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var scheduledAt = DateTimeOffset.UtcNow.AddDays(2);
        var address = CreateAddress();

        // Act
        var booking = Booking.Create(
            customerId,
            serviceId,
            scheduledAt,
            address,
            500m,
            "Please call before arrival.");

        // Assert
        Assert.NotEqual(Guid.Empty, booking.Id);
        Assert.Equal(customerId, booking.CustomerId);
        Assert.Equal(serviceId, booking.ServiceId);
        Assert.Equal(scheduledAt, booking.ScheduledAtUtc);
        Assert.Equal(address, booking.Address);
        Assert.Equal(500m, booking.PriceAtBooking);
        Assert.Equal(
            "Please call before arrival.",
            booking.Notes);
        Assert.Equal(BookingStatus.Pending, booking.Status);
    }

    [Fact]
    public void Create_WithEmptyCustomerId_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() =>
            Booking.Create(
                Guid.Empty,
                Guid.NewGuid(),
                DateTimeOffset.UtcNow.AddDays(1),
                CreateAddress(),
                500m));
    }

    [Fact]
    public void Create_WithEmptyServiceId_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() =>
            Booking.Create(
                Guid.NewGuid(),
                Guid.Empty,
                DateTimeOffset.UtcNow.AddDays(1),
                CreateAddress(),
                500m));
    }

    [Fact]
    public void Create_WithNonUtcDate_ShouldThrowDomainException()
    {
        var nonUtcDate = new DateTimeOffset(
            2026, 12, 1, 10, 0, 0,
            TimeSpan.FromHours(2));

        Assert.Throws<DomainException>(() =>
            Booking.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                nonUtcDate,
                CreateAddress(),
                500m));
    }

    [Fact]
    public void Create_WithNegativePrice_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() =>
            Booking.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTimeOffset.UtcNow.AddDays(1),
                CreateAddress(),
                -100m));
    }

    [Fact]
    public void Create_WithNullAddress_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Booking.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTimeOffset.UtcNow.AddDays(1),
                null!,
                500m));
    }

    [Fact]
    public void Create_ShouldRaiseBookingCreatedEvent()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        // Act
        var booking = Booking.Create(
            customerId,
            serviceId,
            DateTimeOffset.UtcNow.AddDays(1),
            CreateAddress(),
            500m);

        // Assert
        var domainEvent = Assert.Single(booking.DomainEvents);

        var createdEvent =
            Assert.IsType<BookingCreatedEvent>(domainEvent);

        Assert.Equal(booking.Id, createdEvent.BookingId);
        Assert.Equal(customerId, createdEvent.CustomerId);
        Assert.Equal(serviceId, createdEvent.ServiceId);
        Assert.Equal(booking.CreatedAtUtc, createdEvent.OccurredOnUtc);
    }

    [Fact]
    public void Create_WithBlankNotes_ShouldStoreNull()
    {
        var booking = Booking.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTimeOffset.UtcNow.AddDays(1),
            CreateAddress(),
            500m,
            "   ");

        Assert.Null(booking.Notes);
    }

    [Fact]
    public void Confirm_WhenBookingIsPending_ShouldChangeStatusToConfirmed()
    {
        var booking = CreateTestBooking();

        booking.Confirm();

        Assert.Equal(BookingStatus.Confirmed, booking.Status);
    }

    [Fact]
    public void Confirm_WhenBookingIsAlreadyConfirmed_ShouldThrowDomainException()
    {
        var booking = CreateTestBooking();

        booking.Confirm();

        Assert.Throws<DomainException>(() => booking.Confirm());
    }

    [Fact]
    public void Confirm_ShouldRaiseBookingConfirmedEvent()
    {
        var booking = CreateTestBooking();

        booking.Confirm();

        Assert.Contains(
            booking.DomainEvents,
            domainEvent => domainEvent is BookingConfirmedEvent);
    }

    // Test helper
    private static Booking CreateTestBooking()
    {
        return Booking.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTimeOffset.UtcNow.AddDays(2),
            new Address(
                "Cairo",
                "Nasr City",
                "Street 1",
                "15"),
            500m);
    }


    [Fact]
    public void StartService_WhenBookingIsConfirmed_ShouldStartService()
    {
        var booking = CreateTestBooking();

        booking.Confirm();

        booking.StartService();

        Assert.Equal(BookingStatus.InProgress, booking.Status);
    }

    [Fact]
    public void StartService_WhenBookingIsPending_ShouldThrowDomainException()
    {
        var booking = CreateTestBooking();

        Assert.Throws<DomainException>(
            () => booking.StartService());
    }

    [Fact]
    public void StartService_ShouldRaiseBookingStartedEvent()
    {
        var booking = CreateTestBooking();

        booking.Confirm();
        booking.StartService();

        Assert.Contains(
            booking.DomainEvents,
            domainEvent => domainEvent is BookingStartedEvent);
    }

    [Fact]
    public void CompleteService_WhenBookingIsInProgress_ShouldCompleteBooking()
    {
        var booking = CreateTestBooking();

        booking.Confirm();
        booking.StartService();

        booking.CompleteService();

        Assert.Equal(BookingStatus.Completed, booking.Status);
    }

    [Fact]
    public void CompleteService_WhenBookingIsPending_ShouldThrowDomainException()
    {
        var booking = CreateTestBooking();

        Assert.Throws<DomainException>(
            () => booking.CompleteService());
    }

    [Fact]
    public void CompleteService_ShouldRaiseBookingCompletedEvent()
    {
        var booking = CreateTestBooking();

        booking.Confirm();
        booking.StartService();
        booking.CompleteService();

        Assert.Contains(
            booking.DomainEvents,
            domainEvent => domainEvent is BookingCompletedEvent);
    }

    [Theory]
    [InlineData(BookingStatus.Pending)]
    [InlineData(BookingStatus.Confirmed)]
    public void Cancel_WhenBookingHasNotStarted_ShouldCancelBooking(
        BookingStatus initialStatus)
    {
        var booking = CreateTestBooking();

        if (initialStatus == BookingStatus.Confirmed)
        {
            booking.Confirm();
        }

        booking.Cancel();

        Assert.Equal(BookingStatus.Cancelled, booking.Status);
    }

    [Fact]
    public void Cancel_WhenBookingIsInProgress_ShouldThrowDomainException()
    {
        var booking = CreateTestBooking();

        booking.Confirm();
        booking.StartService();

        Assert.Throws<DomainException>(
            () => booking.Cancel());
    }

    [Fact]
    public void Cancel_WhenBookingIsCompleted_ShouldThrowDomainException()
    {
        var booking = CreateTestBooking();

        booking.Confirm();
        booking.StartService();
        booking.CompleteService();

        Assert.Throws<DomainException>(
            () => booking.Cancel());
    }

    [Fact]
    public void Cancel_WhenBookingIsAlreadyCancelled_ShouldThrowDomainException()
    {
        var booking = CreateTestBooking();

        booking.Cancel();

        Assert.Throws<DomainException>(
            () => booking.Cancel());
    }

    [Fact]
    public void Cancel_ShouldRaiseBookingCancelledEvent()
    {
        var booking = CreateTestBooking();

        booking.Cancel();

        Assert.Contains(
            booking.DomainEvents,
            domainEvent => domainEvent is BookingCancelledEvent);
    }


}
