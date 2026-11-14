namespace Tamreed2.Application.Common.Interfaces;

public interface IAuthenticationService
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHash);
}
