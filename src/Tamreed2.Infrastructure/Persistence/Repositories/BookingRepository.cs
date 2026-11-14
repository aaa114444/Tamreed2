using Microsoft.EntityFrameworkCore;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Bookings;

namespace Tamreed2.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository : IBookingRepository
{
    private readonly Tamreed2DbContext _dbContext;

    public BookingRepository(Tamreed2DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken)
    {
        await _dbContext.Bookings.AddAsync(booking, cancellationToken);
    }

    public async Task<Booking?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        return await _dbContext.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(booking => booking.Id == bookingId, cancellationToken);
    }

    public async Task<Booking?> GetByIdForUpdateAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        return await _dbContext.Bookings
            .FirstOrDefaultAsync(booking => booking.Id == bookingId, cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return await _dbContext.Bookings
            .AsNoTracking()
            .Where(booking => booking.CustomerId == customerId)
            .OrderByDescending(booking => booking.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Bookings
            .AsNoTracking()
            .OrderByDescending(booking => booking.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}
