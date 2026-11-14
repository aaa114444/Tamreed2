namespace Tamreed2.Application.Common.Interfaces;

public interface ICurrentUserContext
{
    Guid? UserId { get; }

    string? Email { get; }

    string? Role { get; }

    bool IsAuthenticated { get; }
}
