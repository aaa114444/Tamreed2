
using Tamreed2.Domain.Bookings.ValueObjects;
using Tamreed2.Domain.Common.Exceptions;

namespace Tamreed2.UnitTests.Domain.Bookings.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Create_WithValidValues_ShouldCreateAddress()
    {
        // Arrange & Act
        var address = new Address(
            "Cairo",
            "Nasr City",
            "Makram Ebeid Street",
            "15",
            "Apartment 4");

        // Assert
        Assert.Equal("Cairo", address.City);
        Assert.Equal("Nasr City", address.District);
        Assert.Equal("Makram Ebeid Street", address.Street);
        Assert.Equal("15", address.BuildingNumber);
        Assert.Equal("Apartment 4", address.ApartmentNumber);
    }

    [Fact]
    public void Create_WithoutApartmentNumber_ShouldCreateAddress()
    {
        var address = new Address(
            "Cairo",
            "Nasr City",
            "Makram Ebeid Street",
            "15");

        Assert.Null(address.ApartmentNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidCity_ShouldThrowDomainException(
        string? city)
    {
        Assert.Throws<DomainException>(() =>
            new Address(city!, "Nasr City", "Street 1", "15"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidDistrict_ShouldThrowDomainException(
        string? district)
    {
        Assert.Throws<DomainException>(() =>
            new Address("Cairo", district!, "Street 1", "15"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidStreet_ShouldThrowDomainException(
        string? street)
    {
        Assert.Throws<DomainException>(() =>
            new Address("Cairo", "Nasr City", street!, "15"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidBuildingNumber_ShouldThrowDomainException(
        string? buildingNumber)
    {
        Assert.Throws<DomainException>(() =>
            new Address(
                "Cairo",
                "Nasr City",
                "Street 1",
                buildingNumber!));
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrimValues()
    {
        var address = new Address(
            " Cairo ",
            " Nasr City ",
            " Street 1 ",
            " 15 ",
            " Apartment 4 ");

        Assert.Equal("Cairo", address.City);
        Assert.Equal("Nasr City", address.District);
        Assert.Equal("Street 1", address.Street);
        Assert.Equal("15", address.BuildingNumber);
        Assert.Equal("Apartment 4", address.ApartmentNumber);
    }

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        var address1 = new Address(
            "Cairo", "Nasr City", "Street 1", "15");

        var address2 = new Address(
            "Cairo", "Nasr City", "Street 1", "15");

        Assert.Equal(address1, address2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        var address1 = new Address(
            "Cairo", "Nasr City", "Street 1", "15");

        var address2 = new Address(
            "Cairo", "Heliopolis", "Street 1", "15");

        Assert.NotEqual(address1, address2);
    }

    [Fact]
    public void Equals_WithDifferentApartmentNumbers_ShouldReturnFalse()
    {
        var address1 = new Address(
            "Cairo", "Nasr City", "Street 1", "15", "Apartment 1");

        var address2 = new Address(
            "Cairo", "Nasr City", "Street 1", "15", "Apartment 2");

        Assert.NotEqual(address1, address2);
    }
}
