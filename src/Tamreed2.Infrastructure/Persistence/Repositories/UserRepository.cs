using Microsoft.EntityFrameworkCore;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Users;

namespace Tamreed2.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly Tamreed2DbContext _dbContext;

    public UserRepository(Tamreed2DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(
        Guid userId,
 CancellationToken cancellationToken)
    {
        return await _dbContext.Users
     .AsNoTracking()
     .FirstOrDefaultAsync(
         user => user.Id == userId,
         cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(
 string email,
 CancellationToken cancellationToken)
    {
        return await _dbContext.Users
     .AsNoTracking()
     .FirstOrDefaultAsync(
         user => user.Email == email.ToLowerInvariant(),
         cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(
        CancellationToken cancellationToken)
    {
 return await _dbContext.Users
     .AsNoTracking()
     .OrderBy(user => user.Email)
     .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        User user,
 CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(
 string email,
        CancellationToken cancellationToken)
    {
 return await _dbContext.Users
     .AnyAsync(
  user => user.Email == email.ToLowerInvariant(),
         cancellationToken);
    }
}
