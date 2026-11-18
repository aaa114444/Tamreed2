using Moq;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Application.Services.Commands.UpdateService;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Commands.UpdateService;

public class UpdateServiceCommandHandlerTests
{
    private readonly Mock<IServiceRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly UpdateServiceCommandHandler _handler;

    public UpdateServiceCommandHandlerTests()
    {
        _handler = new UpdateServiceCommandHandler(
            _repository.Object, _unitOfWork.Object);
    }

    private static Service CreateService() =>
        Service.Create("Home Nursing", "Nursing", "Old description",
            new[] { "Old point" }, "??????? ???????",
            "?????", "??? ????", new[] { "???? ?????" });

    private static UpdateServiceCommand UpdateCommand(Guid id) =>
        new(id, "Physiotherapy at Home", "Physiotherapy",
            "Professional physiotherapy services.",
            new[] { "New point" }, "?????? ??????? ???????",
            "???? ?????", "????? ???? ?????",
            new[] { "???? ?????" });

    [Fact]
    public async Task Handle_WithExistingService_ShouldUpdateAndSave()
    {
        var service = CreateService();
        var command = UpdateCommand(service.Id);

        _repository.Setup(x => x.GetByIdForUpdateAsync(
            service.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        _unitOfWork.Setup(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(service.Id, result);
        Assert.Equal("Physiotherapy at Home", service.Title);
        Assert.Equal("Physiotherapy", service.Name);
        Assert.Equal("Professional physiotherapy services.", service.Description);
        Assert.Equal("?????? ??????? ???????", service.TitleAr);
        Assert.Equal(new[] { "New point" }, service.Points);

        _unitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingService_ShouldThrow()
    {
        var id = Guid.NewGuid();

        _repository.Setup(x => x.GetByIdForUpdateAsync(
            id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Service?)null);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(UpdateCommand(id), CancellationToken.None));

        Assert.Contains(id.ToString(), exception.Message);
        _unitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithExistingService_ShouldReturnServiceId()
    {
        var service = CreateService();

        _repository.Setup(x => x.GetByIdForUpdateAsync(
            service.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        _unitOfWork.Setup(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var result = await _handler.Handle(
            UpdateCommand(service.Id), CancellationToken.None);

        Assert.Equal(service.Id, result);
    }
}
