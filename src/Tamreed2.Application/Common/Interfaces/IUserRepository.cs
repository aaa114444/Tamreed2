using Tamreed2.Domain.Users;

namespace Tamreed2.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<User?> GetByIdForUpdateAsync(Guid userId, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
}
