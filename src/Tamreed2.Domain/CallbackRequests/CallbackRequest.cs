using Tamreed2.Domain.Common.Exceptions;
using Tamreed2.Domain.Common.Models;

namespace Tamreed2.Domain.CallbackRequests;

public sealed class CallbackRequest : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string MobileNumber { get; private set; } = null!;
    public List<string> ServicesNeeded { get; private set; } = new();
    public string? Message { get; private set; }
    public CallbackRequestStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset? UpdatedAtUtc { get; private set; }

    private CallbackRequest() { }

    private CallbackRequest(
        string name,
        string mobileNumber,
        IEnumerable<string> servicesNeeded,
        string? message)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required.");

        if (name.Trim().Length > 120)
            throw new DomainException("Name cannot exceed 120 characters.");

        if (string.IsNullOrWhiteSpace(mobileNumber))
            throw new DomainException("Mobile number is required.");

        if (mobileNumber.Trim().Length > 30)
            throw new DomainException("Mobile number cannot exceed 30 characters.");

        ArgumentNullException.ThrowIfNull(servicesNeeded);

        var normalizedServices = servicesNeeded
            .Where(service => !string.IsNullOrWhiteSpace(service))
            .Select(service => service.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (normalizedServices.Count == 0)
            throw new DomainException("At least one service must be selected.");

        if (normalizedServices.Count > 10)
            throw new DomainException("No more than 10 services can be selected.");

        if (normalizedServices.Any(service => service.Length > 150))
            throw new DomainException("A service name cannot exceed 150 characters.");

        if (message?.Length > 1000)
            throw new DomainException("Message cannot exceed 1000 characters.");

        Name = name.Trim();
        MobileNumber = mobileNumber.Trim();
        ServicesNeeded = normalizedServices;
        Message = string.IsNullOrWhiteSpace(message) ? null : message.Trim();
        Status = CallbackRequestStatus.New;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public static CallbackRequest Create(
        string name,
        string mobileNumber,
        IEnumerable<string> servicesNeeded,
        string? message = null)
    {
        return new CallbackRequest(name, mobileNumber, servicesNeeded, message);
    }

    public void ChangeStatus(CallbackRequestStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new DomainException("Invalid callback request status.");

        Status = status;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void MarkAsContacted()
    {
        ChangeStatus(CallbackRequestStatus.Contacted);
    }

    public void MarkAsCompleted()
    {
        ChangeStatus(CallbackRequestStatus.Completed);
    }
}