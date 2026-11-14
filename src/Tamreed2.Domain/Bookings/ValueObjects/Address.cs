
using Tamreed2.Domain.Common.Exceptions;
using Tamreed2.Domain.Common.Models;

namespace Tamreed2.Domain.Bookings.ValueObjects;

public sealed class Address : ValueObject
{
    public string City { get; }
    public string District { get; }
    public string Street { get; }
    public string BuildingNumber { get; }
    public string? ApartmentNumber { get; }

    public Address(
        string city,
        string district,
        string street,
        string buildingNumber,
        string? apartmentNumber = null)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("City is required.");

        if (string.IsNullOrWhiteSpace(district))
            throw new DomainException("District is required.");

        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street is required.");

        if (string.IsNullOrWhiteSpace(buildingNumber))
            throw new DomainException("Building number is required.");

        City = city.Trim();
        District = district.Trim();
        Street = street.Trim();
        BuildingNumber = buildingNumber.Trim();

        ApartmentNumber = string.IsNullOrWhiteSpace(apartmentNumber)
            ? null
            : apartmentNumber.Trim();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return City;
        yield return District;
        yield return Street;
        yield return BuildingNumber;
        yield return ApartmentNumber;
    }
}
