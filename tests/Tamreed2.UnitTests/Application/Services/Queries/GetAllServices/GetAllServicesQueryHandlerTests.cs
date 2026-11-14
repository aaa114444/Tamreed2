
using Moq;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Application.Services.Queries.GetAllServices;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Queries.GetAllServices;

public class GetAllServicesQueryHandlerTests
{
    private readonly Mock<IServiceRepository> _repositoryMock;
    private readonly GetAllServicesQueryHandler _handler;

    public GetAllServicesQueryHandlerTests()
    {
        _repositoryMock = new Mock<IServiceRepository>();

        _handler = new GetAllServicesQueryHandler(
            _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingServices_ShouldReturnAllServices()
    {
        // Arrange
        var services = new List<Service>
        {
            Service.Create(
                "Home Nursing",
                "Professional nursing services at home.",
                500m),

            Service.Create(
                "Physiotherapy",
                "Professional physiotherapy at home.",
                750m)
        };

        _repositoryMock
            .Setup(repository => repository.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(services);

        // Act
        var result = await _handler.Handle(
            new GetAllServicesQuery(),
            CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal("Home Nursing", result[0].Name);
        Assert.Equal(500m, result[0].Price);

        Assert.Equal("Physiotherapy", result[1].Name);
        Assert.Equal(750m, result[1].Price);
    }

    [Fact]
    public async Task Handle_WithNoServices_ShouldReturnEmptyList()
    {
        // Arrange
        _repositoryMock
            .Setup(repository => repository.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Service>());

        // Act
        var result = await _handler.Handle(
            new GetAllServicesQuery(),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldMapAllServiceProperties()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);

        _repositoryMock
            .Setup(repository => repository.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Service> { service });

        // Act
        var result = await _handler.Handle(
            new GetAllServicesQuery(),
            CancellationToken.None);

        // Assert
        var response = Assert.Single(result);

        Assert.Equal(service.Id, response.Id);
        Assert.Equal(service.Name, response.Name);
        Assert.Equal(service.Description, response.Description);
        Assert.Equal(service.Price, response.Price);
        Assert.Equal(service.Status.ToString(), response.Status);
        Assert.Equal(service.CreatedAtUtc, response.CreatedAtUtc);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken = cancellationTokenSource.Token;

        _repositoryMock
            .Setup(repository => repository.GetAllAsync(
                cancellationToken))
            .ReturnsAsync(new List<Service>());

        // Act
        await _handler.Handle(
            new GetAllServicesQuery(),
            cancellationToken);

        // Assert
        _repositoryMock.Verify(
            repository => repository.GetAllAsync(
                cancellationToken),
            Times.Once);
    }
}
