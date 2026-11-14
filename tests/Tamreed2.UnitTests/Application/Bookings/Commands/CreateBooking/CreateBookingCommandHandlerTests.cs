using Moq;
using Tamreed2.Application.Bookings.Commands.CreateBooking;
using Tamreed2.Application.Common.Exceptions;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Bookings;
using Tamreed2.Domain.Services;
using Xunit;

namespace Tamreed2.UnitTests.Application.Bookings.Commands.CreateBooking;

public class CreateBookingCommandHandlerTests
{
    private readonly Mock<IServiceRepository> _serviceRepositoryMock = new();
    private readonly Mock<IBookingRepository> _bookingRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICurrentUserContext> _currentUserContextMock = new();

    private readonly Guid _userId = Guid.NewGuid();
    private readonly CreateBookingCommandHandler _handler;

    public CreateBookingCommandHandlerTests()
    {
        SetupAuthenticatedUser();

        _handler = new CreateBookingCommandHandler(
            _serviceRepositoryMock.Object,
            _bookingRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _currentUserContextMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCreateBooking()
    {
        var service = CreateActiveService();
        var command = CreateValidCommand(service.Id);

        SetupActiveService(service);
        SetupSuccessfulSave();

        Booking? savedBooking = null;

        _bookingRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Booking>(),
                It.IsAny<CancellationToken>()))
            .Callback<Booking, CancellationToken>(
                (booking, _) => savedBooking = booking)
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result);
        Assert.NotNull(savedBooking);
        Assert.Equal(result, savedBooking.Id);
        Assert.Equal(_userId, savedBooking.CustomerId);
        Assert.Equal(command.ServiceId, savedBooking.ServiceId);
        Assert.Equal(command.ScheduledAtUtc, savedBooking.ScheduledAtUtc);
        Assert.Equal(BookingStatus.Pending, savedBooking.Status);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthenticated_ShouldThrowUnauthorizedAccessException()
    {
        var command = CreateValidCommand(Guid.NewGuid());

        _currentUserContextMock
            .Setup(context => context.IsAuthenticated)
            .Returns(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None));

        VerifyBookingWasNotSaved();
    }

    [Fact]
    public async Task Handle_WhenAuthenticatedUserHasNoUserId_ShouldThrowUnauthorizedAccessException()
    {
        var command = CreateValidCommand(Guid.NewGuid());

        _currentUserContextMock
            .Setup(context => context.UserId)
            .Returns((Guid?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None));

        VerifyBookingWasNotSaved();
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsAdmin_ShouldThrowForbiddenAccessException()
    {
        var command = CreateValidCommand(Guid.NewGuid());

        _currentUserContextMock
            .Setup(context => context.Role)
            .Returns("Admin");

        await Assert.ThrowsAsync<ForbiddenAccessException>(
            () => _handler.Handle(command, CancellationToken.None));

        VerifyBookingWasNotSaved();
    }

    [Fact]
    public async Task Handle_WithNonExistingService_ShouldThrowKeyNotFoundException()
    {
        var serviceId = Guid.NewGuid();
        var command = CreateValidCommand(serviceId);

        _serviceRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                serviceId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Service?)null);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains(serviceId.ToString(), exception.Message);
        VerifyBookingWasNotSaved();
    }

    [Fact]
    public async Task Handle_WithInactiveService_ShouldThrowInvalidOperationException()
    {
        var service = CreateActiveService();
        service.Deactivate();

        SetupActiveService(service);

        var command = CreateValidCommand(service.Id);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Equal(
            "The selected service is currently inactive.",
            exception.Message);

        VerifyBookingWasNotSaved();
    }

    [Fact]
    public async Task Handle_ShouldUseServicePriceAsBookingPrice()
    {
        var service = CreateActiveService();
        SetupActiveService(service);
        SetupSuccessfulSave();

        Booking? savedBooking = null;

        _bookingRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Booking>(),
                It.IsAny<CancellationToken>()))
            .Callback<Booking, CancellationToken>(
                (booking, _) => savedBooking = booking)
            .Returns(Task.CompletedTask);

        await _handler.Handle(
            CreateValidCommand(service.Id),
            CancellationToken.None);

        Assert.NotNull(savedBooking);
        Assert.Equal(service.Price, savedBooking.PriceAtBooking);
    }

    [Fact]
    public async Task Handle_ShouldCreateAddressFromCommand()
    {
        var service = CreateActiveService();
        var command = CreateValidCommand(service.Id);

        SetupActiveService(service);
        SetupSuccessfulSave();

        Booking? savedBooking = null;

        _bookingRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Booking>(),
                It.IsAny<CancellationToken>()))
            .Callback<Booking, CancellationToken>(
                (booking, _) => savedBooking = booking)
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(savedBooking);
        Assert.Equal(command.City, savedBooking.Address.City);
        Assert.Equal(command.District, savedBooking.Address.District);
        Assert.Equal(command.Street, savedBooking.Address.Street);
        Assert.Equal(command.BuildingNumber, savedBooking.Address.BuildingNumber);
        Assert.Equal(command.ApartmentNumber, savedBooking.Address.ApartmentNumber);
    }

    [Fact]
    public async Task Handle_ShouldTrimAndPreserveBookingNotes()
    {
        var service = CreateActiveService();
        var command = CreateValidCommand(
            service.Id,
            "  Customer needs special assistance.  ");

        SetupActiveService(service);
        SetupSuccessfulSave();

        Booking? savedBooking = null;

        _bookingRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Booking>(),
                It.IsAny<CancellationToken>()))
            .Callback<Booking, CancellationToken>(
                (booking, _) => savedBooking = booking)
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(savedBooking);
        Assert.Equal(
            "Customer needs special assistance.",
            savedBooking.Notes);
    }

    [Fact]
    public async Task Handle_ShouldSaveChangesAfterAddingBooking()
    {
        var service = CreateActiveService();
        SetupActiveService(service);

        var operationOrder = new List<string>();

        _bookingRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Booking>(),
                It.IsAny<CancellationToken>()))
            .Callback(() => operationOrder.Add("Add"))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .Callback(() => operationOrder.Add("Save"))
            .ReturnsAsync(1);

        await _handler.Handle(
            CreateValidCommand(service.Id),
            CancellationToken.None);

        Assert.Equal(new[] { "Add", "Save" }, operationOrder);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToDependencies()
    {
        var service = CreateActiveService();

        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _serviceRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                service.Id,
                cancellationToken))
            .ReturnsAsync(service);

        _bookingRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Booking>(),
                cancellationToken))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                cancellationToken))
            .ReturnsAsync(1);

        await _handler.Handle(
            CreateValidCommand(service.Id),
            cancellationToken);

        _serviceRepositoryMock.Verify(
            repository => repository.GetByIdAsync(
                service.Id,
                cancellationToken),
            Times.Once);

        _bookingRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Booking>(),
                cancellationToken),
            Times.Once);

        _unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                cancellationToken),
            Times.Once);
    }

    private void SetupAuthenticatedUser()
    {
        _currentUserContextMock
            .Setup(context => context.IsAuthenticated)
            .Returns(true);

        _currentUserContextMock
            .Setup(context => context.UserId)
            .Returns(_userId);

        _currentUserContextMock
            .Setup(context => context.Role)
            .Returns("User");
    }

    private void SetupActiveService(Service service)
    {
        _serviceRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                service.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);
    }

    private void SetupSuccessfulSave()
    {
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
    }

    private void VerifyBookingWasNotSaved()
    {
        _bookingRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Booking>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static Service CreateActiveService()
    {
        return Service.Create(
            "Home Nursing",
            "Professional nursing services at home.",
            500m);
    }

    private static CreateBookingCommand CreateValidCommand(
        Guid serviceId,
        string? notes = "Please call before arrival.")
    {
        return new CreateBookingCommand(
            ServiceId: serviceId,
            ScheduledAtUtc: DateTimeOffset.UtcNow.AddDays(2),
            City: "Cairo",
            District: "New Cairo",
            Street: "Street 90",
            BuildingNumber: "12",
            ApartmentNumber: "5",
            Notes: notes);
    }
}
