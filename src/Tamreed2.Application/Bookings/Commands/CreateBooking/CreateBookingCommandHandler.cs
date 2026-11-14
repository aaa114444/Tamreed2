using MediatR;
using Tamreed2.Application.Common.Exceptions;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Bookings;
using Tamreed2.Domain.Bookings.ValueObjects;
using Tamreed2.Domain.Services;

namespace Tamreed2.Application.Bookings.Commands.CreateBooking;

public sealed class CreateBookingCommandHandler
    : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserContext _currentUserContext;

    public CreateBookingCommandHandler(
        IServiceRepository serviceRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserContext currentUserContext)
    {
        _serviceRepository = serviceRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _currentUserContext = currentUserContext;
    }

    public async Task<Guid> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserContext.IsAuthenticated ||
            !_currentUserContext.UserId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "You must be authenticated to create a booking.");
        }

        if (!string.Equals(
                _currentUserContext.Role,
                "User",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ForbiddenAccessException(
                "Only users can create their own bookings.");
        }

        var customerId = _currentUserContext.UserId.Value;

        var service = await _serviceRepository.GetByIdAsync(
            request.ServiceId,
            cancellationToken);

        if (service is null)
        {
            throw new KeyNotFoundException(
                $"Service with ID '{request.ServiceId}' was not found.");
        }

        if (service.Status != ServiceStatus.Active)
        {
            throw new InvalidOperationException(
                "The selected service is currently inactive.");
        }

        var address = new Address(
            request.City,
            request.District,
            request.Street,
            request.BuildingNumber,
            request.ApartmentNumber);

        var booking = Booking.Create(
            customerId,
            request.ServiceId,
            request.ScheduledAtUtc,
            address,
            service.Price,
            request.Notes);

        await _bookingRepository.AddAsync(
            booking,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return booking.Id;
    }
}
