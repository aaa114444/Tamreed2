using Tamreed2.Domain.Common.Exceptions;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Domain.Services;

public class ServiceTests
{
    private static Service CreateValidService() =>
        Service.Create(
            "Home Nursing", "Home Nursing",
            "Professional care at home.", new[] { "Daily care" },
            "??????? ???????", "??????? ???????",
            "????? ???????? ?? ??????", new[] { "????? ?????" });

    [Fact]
    public void Create_WithValidData_ShouldCreateActiveService()
    {
        var service = CreateValidService();

        Assert.NotEqual(Guid.Empty, service.Id);
        Assert.Equal("Home Nursing", service.Name);
        Assert.Equal("??????? ???????", service.NameAr);
        Assert.Single(service.Points);
        Assert.Equal(ServiceStatus.Active, service.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrowDomainException(string? name)
    {
        Assert.Throws<DomainException>(() =>
            Service.Create("Title", name!, "Description", null,
                "?????", "???", "???", null));
    }

    [Fact]
    public void Create_ShouldTrimContent()
    {
        var service = Service.Create(
            " Title ", " Name ", " Description ", null,
            " ????? ", " ??? ", " ??? ", null);

        Assert.Equal("Title", service.Title);
        Assert.Equal("Name", service.Name);
        Assert.Equal("Description", service.Description);
        Assert.Equal("?????", service.TitleAr);
        Assert.Equal("???", service.NameAr);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateEnglishAndArabicContent()
    {
        var service = CreateValidService();

        service.UpdateDetails(
            "Physiotherapy", "Physiotherapy",
            "Treatment at home.", new[] { "Exercises" },
            "?????? ???????", "?????? ???????",
            "???? ?? ??????", new[] { "??????" });

        Assert.Equal("Physiotherapy", service.Name);
        Assert.Equal("?????? ???????", service.NameAr);
        Assert.Equal("Exercises", service.Points[0]);
        Assert.Equal("??????", service.PointsAr[0]);
    }

    [Fact]
    public void Create_WithTooManyPoints_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() =>
            Service.Create("Title", "Name", "Description",
                Enumerable.Repeat("Point", 21),
                "?????", "???", "???", null));
    }

    [Fact]
    public void Deactivate_ShouldSetInactiveStatus()
    {
        var service = CreateValidService();
        service.Deactivate();

        Assert.Equal(ServiceStatus.Inactive, service.Status);
    }

    [Fact]
    public void Reactivate_ShouldSetActiveStatus()
    {
        var service = CreateValidService();
        service.Deactivate();
        service.Reactivate();

        Assert.Equal(ServiceStatus.Active, service.Status);
    }
}
