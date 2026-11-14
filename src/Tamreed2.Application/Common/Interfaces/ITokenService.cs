namespace Tamreed2.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(Guid userId, string email, string role);

    string GenerateRefreshToken();

    bool ValidateRefreshToken(string token);
}
