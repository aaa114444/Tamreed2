
using Tamreed2.Domain.Common.Exceptions;
using Tamreed2.Domain.Common.Models;

namespace Tamreed2.Domain.Services;

public sealed class Service : AggregateRoot
{
    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public decimal Price { get; private set; }

    public ServiceStatus Status { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    private Service()
    {
    }

    private Service(
        string name,
        string description,
        decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Service name is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Service description is required.");

        if (price <= 0)
            throw new DomainException(
                "Service price must be greater than zero.");

        Name = name.Trim();
        Description = description.Trim();
        Price = price;

        Status = ServiceStatus.Active;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public static Service Create(
        string name,
        string description,
        decimal price)
    {
        return new Service(name, description, price);
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new DomainException(
                "Service price must be greater than zero.");

        Price = newPrice;
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException(
                "Service description is required.");

        Description = description.Trim();
    }

    // Update name, description, and price together.
    public void UpdateDetails(
        string name,
        string description,
        decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(
                "Service name is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException(
                "Service description is required.");

        if (price <= 0)
            throw new DomainException(
                "Service price must be greater than zero.");

        Name = name.Trim();
        Description = description.Trim();
        Price = price;
    }

    public void Deactivate()
    {
        if (Status == ServiceStatus.Inactive)
            throw new DomainException(
                "Service is already inactive.");

        Status = ServiceStatus.Inactive;
    }

    public void Reactivate()
    {
        if (Status == ServiceStatus.Active)
            throw new DomainException(
                "Service is already active.");

        Status = ServiceStatus.Active;
    }
}
