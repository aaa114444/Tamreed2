using System.Security.Claims;
using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Infrastructure.Authentication;

public sealed class CurrentUserContext : ICurrentUserContext
{
    private readonly Guid? _userId;
    private readonly string? _email;
    private readonly string? _role;
    private readonly bool _isAuthenticated;

    public CurrentUserContext()
    {
        _userId = null;
        _email = null;
        _role = null;
        _isAuthenticated = false;
    }

    public CurrentUserContext(ClaimsPrincipal user)
    {
        if (user?.Identity?.IsAuthenticated ?? false)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _userId = Guid.TryParse(userIdClaim, out var userId) ? userId : null;
            _email = user.FindFirst(ClaimTypes.Email)?.Value;
            _role = user.FindFirst(ClaimTypes.Role)?.Value;
            _isAuthenticated = true;
        }
        else
        {
            _userId = null;
            _email = null;
            _role = null;
            _isAuthenticated = false;
        }
    }

    public Guid? UserId => _userId;

    public string? Email => _email;

    public string? Role => _role;

    public bool IsAuthenticated => _isAuthenticated;
}
