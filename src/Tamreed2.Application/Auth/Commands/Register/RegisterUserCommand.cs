using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Users;

namespace Tamreed2.Application.Auth.Commands.Register;

public sealed class RegisterUserCommand : IRequest<AuthResponse>
{
    public RegisterUserCommand(
        string email,
        string password,
        string firstName,
        string lastName)
    {
 Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
    }

    public string Email { get; }
    public string Password { get; }
    public string FirstName { get; }
    public string LastName { get; }
}

public sealed record AuthResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string Role,
    string AccessToken,
    string RefreshToken);

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
 IUserRepository userRepository,
 IAuthenticationService authenticationService,
        ITokenService tokenService,
 IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
 _authenticationService = authenticationService;
 _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        // Check if email already exists
        var emailExists = await _userRepository.EmailExistsAsync(
     request.Email,
     cancellationToken);

 if (emailExists)
        {
            throw new InvalidOperationException(
         $"Email '{request.Email}' is already registered.");
        }

        // Hash password
 var passwordHash = _authenticationService.HashPassword(request.Password);

        // Create user
        var user = User.Create(
     request.Email,
     passwordHash,
            request.FirstName,
     request.LastName,
            UserRole.User);

        // Add to repository
        await _userRepository.AddAsync(user, cancellationToken);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Generate tokens
        var accessToken = _tokenService.GenerateAccessToken(
     user.Id,
            user.Email,
     user.Role.ToString());

        var refreshToken = _tokenService.GenerateRefreshToken();

 return new AuthResponse(
     user.Id,
     user.Email,
     user.FirstName,
     user.LastName,
     user.Role.ToString(),
     accessToken,
     refreshToken);
    }
}

