using Tamreed2.Domain.Bookings;

namespace Tamreed2.Application.Common.Interfaces;

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken cancellationToken);

    Task<Booking?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken);

    Task<Booking?> GetByIdForUpdateAsync(Guid bookingId, CancellationToken cancellationToken);

    Task<IReadOnlyList<Booking>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);

    Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken);
}
