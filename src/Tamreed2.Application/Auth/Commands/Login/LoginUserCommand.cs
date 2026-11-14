using MediatR;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Application.Auth.Commands.Register;

namespace Tamreed2.Application.Auth.Commands.Login;

public sealed class LoginUserCommand : IRequest<AuthResponse>
{
    public LoginUserCommand(string email, string password)
    {
        Email = email;
 Password = password;
    }

    public string Email { get; }
    public string Password { get; }
}

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserCommandHandler(
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
 LoginUserCommand request,
 CancellationToken cancellationToken)
    {
        // Find user by email
 var user = await _userRepository.GetByEmailAsync(
     request.Email,
     cancellationToken);

        if (user is null)
        {
     throw new InvalidOperationException(
  "Invalid email or password.");
 }

 // Check if user is active
 if (!user.IsActive)
        {
     throw new InvalidOperationException(
  "User account is inactive.");
 }

        // Verify password
        var isPasswordValid = _authenticationService.VerifyPassword(
     request.Password,
            user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new InvalidOperationException(
         "Invalid email or password.");
        }

        // Record login
 user.RecordLogin();

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
