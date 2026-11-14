
using Moq;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Application.Services.Commands.CreateService;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Commands.CreateService;

public class CreateServiceCommandHandlerTests
{
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateServiceCommandHandler _handler;

    public CreateServiceCommandHandlerTests()
    {
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreateServiceCommandHandler(
            _serviceRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateAndAddService()
    {
        // Arrange
        var command = new CreateServiceCommand(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);

        Service? addedService = null;

        _serviceRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Service>(),
                It.IsAny<CancellationToken>()))
            .Callback<Service, CancellationToken>(
                (service, _) => addedService = service)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(addedService);
        Assert.Equal("Home Nursing", addedService.Name);
        Assert.Equal(
            "Professional nursing services at home.",
            addedService.Description);
        Assert.Equal(500m, addedService.Price);
        Assert.Equal(ServiceStatus.Active, addedService.Status);

        _serviceRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Service>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldSaveChanges()
    {
        // Arrange
        var command = new CreateServiceCommand(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);

        _serviceRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Service>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnCreatedServiceId()
    {
        // Arrange
        var command = new CreateServiceCommand(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);

        Service? addedService = null;

        _serviceRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Service>(),
                It.IsAny<CancellationToken>()))
            .Callback<Service, CancellationToken>(
                (service, _) => addedService = service)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotNull(addedService);
        Assert.Equal(addedService.Id, result);
        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ShouldNotSaveChanges()
    {
        // Arrange
        var command = new CreateServiceCommand(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);

        _serviceRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Service>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException(
                "Repository operation failed."));

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal("Repository operation failed.", exception.Message);

        _unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToDependencies()
    {
        // Arrange
        var command = new CreateServiceCommand(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);

        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _serviceRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Service>(),
                cancellationToken))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _serviceRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Service>(),
                cancellationToken),
            Times.Once);

        _unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                cancellationToken),
            Times.Once);
    }
}
