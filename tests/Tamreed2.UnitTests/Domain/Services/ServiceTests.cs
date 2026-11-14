
using Tamreed2.Domain.Common.Exceptions;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Domain.Services;

public class ServiceTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateActiveService()
    {
        // Arrange
        var name = "Home Nursing";
        var description = "Professional nursing at home.";
        var price = 500m;

        // Act
        var service = Service.Create(name, description, price);

        // Assert
        Assert.NotEqual(Guid.Empty, service.Id);
        Assert.Equal(name, service.Name);
        Assert.Equal(description, service.Description);
        Assert.Equal(price, service.Price);
        Assert.Equal(ServiceStatus.Active, service.Status);
        Assert.True(service.CreatedAtUtc <= DateTimeOffset.UtcNow);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrowDomainException(
        string? name)
    {
        // Arrange & Act & Assert
        Assert.Throws<DomainException>(() =>
            Service.Create(name!, "Valid description", 500m));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidDescription_ShouldThrowDomainException(
        string? description)
    {
        // Arrange & Act & Assert
        Assert.Throws<DomainException>(() =>
            Service.Create("Home Nursing", description!, 500m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidPrice_ShouldThrowDomainException(
        decimal price)
    {
        // Arrange & Act & Assert
        Assert.Throws<DomainException>(() =>
            Service.Create("Home Nursing", "Valid description", price));
    }

    [Fact]
    public void Create_ShouldTrimNameAndDescription()
    {
        // Arrange & Act
        var service = Service.Create(
            "  Home Nursing  ",
            "  Professional nursing at home.  ",
            500m);

        // Assert
        Assert.Equal("Home Nursing", service.Name);
        Assert.Equal("Professional nursing at home.", service.Description);
    }

    [Fact]
    public void UpdatePrice_WithValidPrice_ShouldUpdatePrice()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing at home.",
            500m);

        // Act
        service.UpdatePrice(600m);

        // Assert
        Assert.Equal(600m, service.Price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void UpdatePrice_WithInvalidPrice_ShouldThrowDomainException(
        decimal newPrice)
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing at home.",
            500m);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            service.UpdatePrice(newPrice));
    }

    [Fact]
    public void UpdateDescription_WithValidDescription_ShouldUpdateDescription()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Old description",
            500m);

        // Act
        service.UpdateDescription("New description");

        // Assert
        Assert.Equal("New description", service.Description);
    }

    [Fact]
    public void UpdateDescription_ShouldTrimDescription()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Old description",
            500m);

        // Act
        service.UpdateDescription("  New description  ");

        // Assert
        Assert.Equal("New description", service.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateDescription_WithInvalidDescription_ShouldThrowDomainException(
        string? description)
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Old description",
            500m);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            service.UpdateDescription(description!));
    }

    // UpdateDetails tests

    [Fact]
    public void UpdateDetails_WithValidData_ShouldUpdateAllProperties()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Old description",
            500m);

        // Act
        service.UpdateDetails(
            "Physiotherapy at Home",
            "Professional physiotherapy services.",
            750m);

        // Assert
        Assert.Equal("Physiotherapy at Home", service.Name);
        Assert.Equal(
            "Professional physiotherapy services.",
            service.Description);
        Assert.Equal(750m, service.Price);
    }

    [Fact]
    public void UpdateDetails_ShouldTrimNameAndDescription()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Old description",
            500m);

        // Act
        service.UpdateDetails(
            "  Physiotherapy at Home  ",
            "  Professional physiotherapy services.  ",
            750m);

        // Assert
        Assert.Equal("Physiotherapy at Home", service.Name);
        Assert.Equal(
            "Professional physiotherapy services.",
            service.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateDetails_WithInvalidName_ShouldThrowDomainException(
        string? name)
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Old description",
            500m);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            service.UpdateDetails(
                name!,
                "New description",
                750m));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateDetails_WithInvalidDescription_ShouldThrowDomainException(
        string? description)
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Old description",
            500m);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            service.UpdateDetails(
                "New name",
                description!,
                750m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void UpdateDetails_WithInvalidPrice_ShouldThrowDomainException(
        decimal price)
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Old description",
            500m);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            service.UpdateDetails(
                "New name",
                "New description",
                price));
    }

    [Fact]
    public void UpdateDetails_WithInvalidData_ShouldNotModifyExistingProperties()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Old description",
            500m);

        // Act
        Assert.Throws<DomainException>(() =>
            service.UpdateDetails(
                "New name",
                "",
                750m));

        // Assert
        Assert.Equal("Home Nursing", service.Name);
        Assert.Equal("Old description", service.Description);
        Assert.Equal(500m, service.Price);
    }

    // Deactivate tests

    [Fact]
    public void Deactivate_WhenActive_ShouldSetStatusToInactive()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing at home.",
            500m);

        // Act
        service.Deactivate();

        // Assert
        Assert.Equal(ServiceStatus.Inactive, service.Status);
    }

    [Fact]
    public void Deactivate_WhenAlreadyInactive_ShouldThrowDomainException()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing at home.",
            500m);

        service.Deactivate();

        // Act & Assert
        Assert.Throws<DomainException>(() => service.Deactivate());
    }

    // Reactivate tests

    [Fact]
    public void Reactivate_WhenInactive_ShouldSetStatusToActive()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing at home.",
            500m);

        service.Deactivate();

        // Act
        service.Reactivate();

        // Assert
        Assert.Equal(ServiceStatus.Active, service.Status);
    }

    [Fact]
    public void Reactivate_WhenAlreadyActive_ShouldThrowDomainException()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing at home.",
            500m);

        // Act & Assert
        Assert.Throws<DomainException>(() => service.Reactivate());
    }
}
