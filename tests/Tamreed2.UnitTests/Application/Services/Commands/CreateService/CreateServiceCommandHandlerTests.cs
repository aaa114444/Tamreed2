using Moq;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Application.Services.Commands.CreateService;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Commands.CreateService;

public class CreateServiceCommandHandlerTests
{
    private readonly Mock<IServiceRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly CreateServiceCommandHandler _handler;

    public CreateServiceCommandHandlerTests()
    {
        _handler = new CreateServiceCommandHandler(
            _repository.Object, _unitOfWork.Object);
    }

    private static CreateServiceCommand ValidCommand() =>
        new("Home Nursing", "Nursing", "Description",
            new[] { "Point 1" }, "??????? ???????",
            "?????", "?????", new[] { "?????? ??????" });

    private void SetupDependencies()
    {
        _repository.Setup(x => x.AddAsync(
            It.IsAny<Service>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWork.Setup(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateAndAddService()
    {
        SetupDependencies();
        Service? added = null;

        _repository.Setup(x => x.AddAsync(
            It.IsAny<Service>(), It.IsAny<CancellationToken>()))
            .Callback<Service, CancellationToken>((s, _) => added = s)
            .Returns(Task.CompletedTask);

        await _handler.Handle(ValidCommand(), CancellationToken.None);

        Assert.NotNull(added);
        Assert.Equal("Home Nursing", added.Title);
        Assert.Equal("Description", added.Description);
        Assert.Equal("??????? ???????", added.TitleAr);
        Assert.Equal(ServiceStatus.Active, added.Status);
        _repository.Verify(x => x.AddAsync(
            It.IsAny<Service>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldSaveChanges()
    {
        SetupDependencies();
        await _handler.Handle(ValidCommand(), CancellationToken.None);

        _unitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnCreatedServiceId()
    {
        SetupDependencies();
        Service? added = null;

        _repository.Setup(x => x.AddAsync(
            It.IsAny<Service>(), It.IsAny<CancellationToken>()))
            .Callback<Service, CancellationToken>((s, _) => added = s)
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(
            ValidCommand(), CancellationToken.None);

        Assert.NotNull(added);
        Assert.Equal(added.Id, result);
        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ShouldNotSaveChanges()
    {
        _repository.Setup(x => x.AddAsync(
            It.IsAny<Service>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Repository operation failed."));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(ValidCommand(), CancellationToken.None));

        Assert.Equal("Repository operation failed.", exception.Message);
        _unitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken()
    {
        SetupDependencies();
        using var source = new CancellationTokenSource();

        await _handler.Handle(ValidCommand(), source.Token);

        _repository.Verify(x => x.AddAsync(
            It.IsAny<Service>(), source.Token), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(
            source.Token), Times.Once);
    }
}


