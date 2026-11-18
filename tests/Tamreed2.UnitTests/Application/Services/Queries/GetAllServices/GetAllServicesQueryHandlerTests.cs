using Moq;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Application.Services.Queries.GetAllServices;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Queries.GetAllServices;

public class GetAllServicesQueryHandlerTests
{
    private readonly Mock<IServiceRepository> _repositoryMock = new();
    private readonly GetAllServicesQueryHandler _handler;

    public GetAllServicesQueryHandlerTests()
    {
        _handler = new GetAllServicesQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingServices_ShouldReturnAllServices()
    {
        var services = new List<Service>
        {
            Service.Create(
                "Home Nursing", "Nursing",
                "Professional nursing services at home.",
                new[] { "Qualified care" },
                "??????? ???????", "?????",
                "????? ????? ??????", new[] { "????? ?????" }),

            Service.Create(
                "Physiotherapy", "Physiotherapy",
                "Professional physiotherapy at home.",
                new[] { "Personalized treatment" },
                "?????? ???????", "???? ?????",
                "????? ???? ????? ??????", new[] { "???? ????" })
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(services);

        var result = await _handler.Handle(
            new GetAllServicesQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("Home Nursing", result[0].Title);
        Assert.Equal("Physiotherapy", result[1].Name);
    }

    [Fact]
    public async Task Handle_WithNoServices_ShouldReturnEmptyList()
    {
        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Service>());

        var result = await _handler.Handle(
            new GetAllServicesQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldMapAllServiceProperties()
    {
        var service = Service.Create(
            "Home Nursing", "Nursing",
            "Professional nursing services at home.",
            new[] { "Qualified care" },
            "??????? ???????", "?????",
            "????? ????? ??????", new[] { "????? ?????" });

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Service> { service });

        var result = await _handler.Handle(
            new GetAllServicesQuery(), CancellationToken.None);

        var response = Assert.Single(result);

        Assert.Equal(service.Id, response.Id);
        Assert.Equal(service.Title, response.Title);
        Assert.Equal(service.Name, response.Name);
        Assert.Equal(service.Description, response.Description);
        Assert.Equal(service.Points, response.Points);
        Assert.Equal(service.TitleAr, response.TitleAr);
        Assert.Equal(service.NameAr, response.NameAr);
        Assert.Equal(service.DescriptionAr, response.DescriptionAr);
        Assert.Equal(service.PointsAr, response.PointsAr);
        Assert.Equal(service.Status.ToString(), response.Status);
        Assert.Equal(service.CreatedAtUtc, response.CreatedAtUtc);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _repositoryMock
            .Setup(r => r.GetAllAsync(cancellationToken))
            .ReturnsAsync(new List<Service>());

        await _handler.Handle(new GetAllServicesQuery(), cancellationToken);

        _repositoryMock.Verify(
            r => r.GetAllAsync(cancellationToken), Times.Once);
    }
}

