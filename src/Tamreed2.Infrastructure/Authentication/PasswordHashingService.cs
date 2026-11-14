using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Infrastructure.Authentication;

public sealed class PasswordHashingService : IAuthenticationService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        try
        {
     return BCrypt.Net.BCrypt.Verify(password, passwordHash);
 }
        catch
        {
     return false;
        }
    }
}
