
using Moq;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Application.Services.Queries.GetServiceById;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandlerTests
{
    private readonly Mock<IServiceRepository> _repositoryMock;
    private readonly GetServiceByIdQueryHandler _handler;

    public GetServiceByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IServiceRepository>();

        _handler = new GetServiceByIdQueryHandler(
            _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingService_ShouldReturnServiceResponse()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);

        var query = new GetServiceByIdQuery(service.Id);

        _repositoryMock
            .Setup(repository => repository.GetByIdAsync(
                service.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        // Act
        var result = await _handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(service.Id, result.Id);
        Assert.Equal(service.Name, result.Name);
        Assert.Equal(service.Description, result.Description);
        Assert.Equal(service.Price, result.Price);
        Assert.Equal(service.Status.ToString(), result.Status);
        Assert.Equal(service.CreatedAtUtc, result.CreatedAtUtc);
    }

    [Fact]
    public async Task Handle_WithNonExistingService_ShouldReturnNull()
    {
        // Arrange
        var serviceId = Guid.NewGuid();
        var query = new GetServiceByIdQuery(serviceId);

        _repositoryMock
            .Setup(repository => repository.GetByIdAsync(
                serviceId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Service?)null);

        // Act
        var result = await _handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithRequestedServiceId()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);

        var query = new GetServiceByIdQuery(service.Id);

        _repositoryMock
            .Setup(repository => repository.GetByIdAsync(
                service.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            repository => repository.GetByIdAsync(
                service.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);

        var query = new GetServiceByIdQuery(service.Id);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken = cancellationTokenSource.Token;

        _repositoryMock
            .Setup(repository => repository.GetByIdAsync(
                service.Id,
                cancellationToken))
            .ReturnsAsync(service);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _repositoryMock.Verify(
            repository => repository.GetByIdAsync(
                service.Id,
                cancellationToken),
            Times.Once);
    }
}
