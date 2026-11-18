using Tamreed2.Domain.Bookings.Events;
using Tamreed2.Domain.Bookings.ValueObjects;
using Tamreed2.Domain.Common.Exceptions;
using Tamreed2.Domain.Common.Models;

namespace Tamreed2.Domain.Bookings;

public sealed class Booking : AggregateRoot
{
    public Guid CustomerId { get; private set; }
    public Guid ServiceId { get; private set; }
    public DateTimeOffset ScheduledAtUtc { get; private set; }
    public Address Address { get; private set; } = null!;
    public string? Notes { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }

    private Booking()
    {
    }

    private Booking(
        Guid customerId,
        Guid serviceId,
        DateTimeOffset scheduledAtUtc,
        Address address,
        string? notes)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("Customer ID cannot be empty.");

        if (serviceId == Guid.Empty)
            throw new DomainException("Service ID cannot be empty.");

        if (scheduledAtUtc.Offset != TimeSpan.Zero)
            throw new DomainException("Scheduled date and time must be in UTC.");

        if (scheduledAtUtc <= DateTimeOffset.UtcNow)
            throw new DomainException("Booking must be scheduled for a future date and time.");

        ArgumentNullException.ThrowIfNull(address);

        CustomerId = customerId;
        ServiceId = serviceId;
        ScheduledAtUtc = scheduledAtUtc;
        Address = address;

        Notes = string.IsNullOrWhiteSpace(notes)
            ? null
            : notes.Trim();

        Status = BookingStatus.Pending;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public static Booking Create(
        Guid customerId,
        Guid serviceId,
        DateTimeOffset scheduledAtUtc,
        Address address,
        string? notes = null)
    {
        var booking = new Booking(
            customerId,
            serviceId,
            scheduledAtUtc,
            address,
            notes);

        booking.AddDomainEvent(
            new BookingCreatedEvent(
                booking.Id,
                booking.CustomerId,
                booking.ServiceId,
                booking.CreatedAtUtc));

        return booking;
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new DomainException("Only pending bookings can be confirmed.");

        Status = BookingStatus.Confirmed;

        AddDomainEvent(new BookingConfirmedEvent(Id, DateTimeOffset.UtcNow));
    }

    public void StartService()
    {
        if (Status != BookingStatus.Confirmed)
            throw new DomainException("Only confirmed bookings can start.");

        Status = BookingStatus.InProgress;

        AddDomainEvent(new BookingStartedEvent(Id, DateTimeOffset.UtcNow));
    }

    public void CompleteService()
    {
        if (Status != BookingStatus.InProgress)
            throw new DomainException("Only in-progress bookings can be completed.");

        Status = BookingStatus.Completed;

        AddDomainEvent(new BookingCompletedEvent(Id, DateTimeOffset.UtcNow));
    }

    public void Cancel()
    {
        if (Status != BookingStatus.Pending &&
            Status != BookingStatus.Confirmed)
        {
            throw new DomainException(
                "Only pending or confirmed bookings can be cancelled.");
        }

        Status = BookingStatus.Cancelled;

        AddDomainEvent(new BookingCancelledEvent(Id, DateTimeOffset.UtcNow));
    }
}
