
using Moq;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Application.Services.Commands.UpdateService;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Commands.UpdateService;

public class UpdateServiceCommandHandlerTests
{
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateServiceCommandHandler _handler;

    public UpdateServiceCommandHandlerTests()
    {
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new UpdateServiceCommandHandler(
            _serviceRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingService_ShouldUpdateServiceAndSaveChanges()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Old description",
            500m);

        var serviceId = service.Id;

        var command = new UpdateServiceCommand(
            serviceId,
            "Physiotherapy at Home",
            "Professional physiotherapy services.",
            750m);

        _serviceRepositoryMock
            .Setup(repository => repository.GetByIdForUpdateAsync(
                serviceId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Equal(service.Id, result);
        Assert.Equal("Physiotherapy at Home", service.Name);
        Assert.Equal(
            "Professional physiotherapy services.",
            service.Description);
        Assert.Equal(750m, service.Price);

        _serviceRepositoryMock.Verify(
            repository => repository.GetByIdForUpdateAsync(
                serviceId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingService_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var serviceId = Guid.NewGuid();

        var command = new UpdateServiceCommand(
            serviceId,
            "Physiotherapy at Home",
            "Professional physiotherapy services.",
            750m);

        _serviceRepositoryMock
            .Setup(repository => repository.GetByIdForUpdateAsync(
                serviceId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Service?)null);

        // Act
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(
                command,
                CancellationToken.None));

        // Assert
        Assert.Contains(serviceId.ToString(), exception.Message);

        _unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithExistingService_ShouldReturnServiceId()
    {
        // Arrange
        var service = Service.Create(
            "Home Nursing",
            "Professional nursing at home.",
            500m);

        var serviceId = service.Id;

        var command = new UpdateServiceCommand(
            serviceId,
            "Home Nursing",
            "Updated nursing description.",
            600m);

        _serviceRepositoryMock
            .Setup(repository => repository.GetByIdForUpdateAsync(
                serviceId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Equal(serviceId, result);

        _unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
