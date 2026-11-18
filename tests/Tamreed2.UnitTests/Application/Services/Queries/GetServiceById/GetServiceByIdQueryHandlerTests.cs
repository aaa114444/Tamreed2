using Moq;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Application.Services.Queries.GetServiceById;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandlerTests
{
    private readonly Mock<IServiceRepository> _repositoryMock = new();
    private readonly GetServiceByIdQueryHandler _handler;

    public GetServiceByIdQueryHandlerTests()
    {
        _handler = new GetServiceByIdQueryHandler(_repositoryMock.Object);
    }

    private static Service CreateService() =>
        Service.Create(
            "Home Nursing", "Home Nursing",
            "Professional nursing services at home.", new[] { "Daily care" },
            "??????? ???????", "??????? ???????",
            "????? ??????? ???????? ???????", new[] { "????? ?????" });

    [Fact]
    public async Task Handle_WithExistingService_ShouldReturnServiceResponse()
    {
        var service = CreateService();
        _repositoryMock.Setup(r => r.GetByIdAsync(
            service.Id, It.IsAny<CancellationToken>())).ReturnsAsync(service);

        var result = await _handler.Handle(
            new GetServiceByIdQuery(service.Id), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(service.Id, result.Id);
        Assert.Equal(service.Title, result.Title);
        Assert.Equal(service.Name, result.Name);
        Assert.Equal(service.Description, result.Description);
        Assert.Equal(service.Points, result.Points);
        Assert.Equal(service.TitleAr, result.TitleAr);
        Assert.Equal(service.NameAr, result.NameAr);
        Assert.Equal(service.DescriptionAr, result.DescriptionAr);
        Assert.Equal(service.PointsAr, result.PointsAr);
        Assert.Equal(service.Status.ToString(), result.Status);
        Assert.Equal(service.CreatedAtUtc, result.CreatedAtUtc);
    }

    [Fact]
    public async Task Handle_WithNonExistingService_ShouldReturnNull()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(
            id, It.IsAny<CancellationToken>())).ReturnsAsync((Service?)null);

        var result = await _handler.Handle(
            new GetServiceByIdQuery(id), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithRequestedServiceId()
    {
        var service = CreateService();
        _repositoryMock.Setup(r => r.GetByIdAsync(
            service.Id, It.IsAny<CancellationToken>())).ReturnsAsync(service);

        await _handler.Handle(
            new GetServiceByIdQuery(service.Id), CancellationToken.None);

        _repositoryMock.Verify(r => r.GetByIdAsync(
            service.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        var service = CreateService();
        using var cts = new CancellationTokenSource();

        _repositoryMock.Setup(r => r.GetByIdAsync(
            service.Id, cts.Token)).ReturnsAsync(service);

        await _handler.Handle(
            new GetServiceByIdQuery(service.Id), cts.Token);

        _repositoryMock.Verify(r => r.GetByIdAsync(
            service.Id, cts.Token), Times.Once);
    }
}
