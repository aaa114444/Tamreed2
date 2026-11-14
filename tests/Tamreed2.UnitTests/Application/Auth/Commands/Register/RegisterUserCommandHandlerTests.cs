using Moq;
using Tamreed2.Application.Auth.Commands.Register;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Users;
using Xunit;

namespace Tamreed2.UnitTests.Application.Auth.Commands.Register;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAuthenticationService> _authenticationServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _authenticationServiceMock = new Mock<IAuthenticationService>();
 _tokenServiceMock = new Mock<ITokenService>();
 _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new RegisterUserCommandHandler(
     _userRepositoryMock.Object,
     _authenticationServiceMock.Object,
     _tokenServiceMock.Object,
     _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateUserAndReturnTokens()
    {
        // Arrange
        var command = new RegisterUserCommand(
     "john@example.com",
     "Password123!",
     "John",
     "Doe");

        var passwordHash = "$2a$12$hashedPassword";
        var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
        var refreshToken = "refresh_token_value";

        _userRepositoryMock
     .Setup(repo => repo.EmailExistsAsync(
  "john@example.com",
  It.IsAny<CancellationToken>()))
     .ReturnsAsync(false);

        _authenticationServiceMock
     .Setup(service => service.HashPassword("Password123!"))
     .Returns(passwordHash);

        User? registeredUser = null;

        _userRepositoryMock
     .Setup(repo => repo.AddAsync(
  It.IsAny<User>(),
  It.IsAny<CancellationToken>()))
     .Callback<User, CancellationToken>(
  (user, _) => registeredUser = user)
     .Returns(Task.CompletedTask);

        _unitOfWorkMock
     .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
     .ReturnsAsync(1);

 _tokenServiceMock
     .Setup(service => service.GenerateAccessToken(
  It.IsAny<Guid>(),
  "john@example.com",
  "User"))
     .Returns(accessToken);

        _tokenServiceMock
     .Setup(service => service.GenerateRefreshToken())
     .Returns(refreshToken);

        // Act
 var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
 Assert.NotNull(result);
        Assert.Equal("john@example.com", result.Email);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("User", result.Role);
        Assert.Equal(accessToken, result.AccessToken);
        Assert.Equal(refreshToken, result.RefreshToken);
 Assert.NotEqual(Guid.Empty, result.UserId);

 _userRepositoryMock.Verify(
     repo => repo.AddAsync(
  It.IsAny<User>(),
  It.IsAny<CancellationToken>()),
     Times.Once);

 _unitOfWorkMock.Verify(
     uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
     Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldThrowInvalidOperationException()
    {
 // Arrange
        var command = new RegisterUserCommand(
     "existing@example.com",
     "Password123!",
     "John",
     "Doe");

 _userRepositoryMock
     .Setup(repo => repo.EmailExistsAsync(
  "existing@example.com",
  It.IsAny<CancellationToken>()))
     .ReturnsAsync(true);

        // Act & Assert
 var exception = await Assert.ThrowsAsync<InvalidOperationException>(
     () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("already registered", exception.Message);

 _userRepositoryMock.Verify(
     repo => repo.AddAsync(
  It.IsAny<User>(),
  It.IsAny<CancellationToken>()),
     Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldHashPassword()
    {
        // Arrange
        var command = new RegisterUserCommand(
     "john@example.com",
     "Password123!",
     "John",
     "Doe");

 var passwordHash = "$2a$12$hashedPassword";

        _userRepositoryMock
     .Setup(repo => repo.EmailExistsAsync(
  It.IsAny<string>(),
  It.IsAny<CancellationToken>()))
     .ReturnsAsync(false);

 _authenticationServiceMock
     .Setup(service => service.HashPassword("Password123!"))
     .Returns(passwordHash);

        _userRepositoryMock
     .Setup(repo => repo.AddAsync(
  It.IsAny<User>(),
  It.IsAny<CancellationToken>()))
     .Returns(Task.CompletedTask);

        _unitOfWorkMock
     .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
     .ReturnsAsync(1);

 _tokenServiceMock
     .Setup(service => service.GenerateAccessToken(
  It.IsAny<Guid>(),
  It.IsAny<string>(),
  It.IsAny<string>()))
     .Returns("token");

 _tokenServiceMock
     .Setup(service => service.GenerateRefreshToken())
     .Returns("refresh");

        // Act
 await _handler.Handle(command, CancellationToken.None);

        // Assert
 _authenticationServiceMock.Verify(
     service => service.HashPassword("Password123!"),
     Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToDependencies()
    {
        // Arrange
        var command = new RegisterUserCommand(
     "john@example.com",
     "Password123!",
     "John",
     "Doe");

        using var cancellationTokenSource = new CancellationTokenSource();
 var cancellationToken = cancellationTokenSource.Token;

 _userRepositoryMock
     .Setup(repo => repo.EmailExistsAsync(
  "john@example.com",
  cancellationToken))
     .ReturnsAsync(false);

 _authenticationServiceMock
     .Setup(service => service.HashPassword(It.IsAny<string>()))
     .Returns("hash");

        _userRepositoryMock
     .Setup(repo => repo.AddAsync(
  It.IsAny<User>(),
  cancellationToken))
     .Returns(Task.CompletedTask);

        _unitOfWorkMock
     .Setup(uow => uow.SaveChangesAsync(cancellationToken))
     .ReturnsAsync(1);

 _tokenServiceMock
     .Setup(service => service.GenerateAccessToken(
  It.IsAny<Guid>(),
  It.IsAny<string>(),
  It.IsAny<string>()))
     .Returns("token");

        _tokenServiceMock
     .Setup(service => service.GenerateRefreshToken())
     .Returns("refresh");

        // Act
 await _handler.Handle(command, cancellationToken);

 // Assert
 _userRepositoryMock.Verify(
     repo => repo.EmailExistsAsync(
  "john@example.com",
  cancellationToken),
     Times.Once);

 _userRepositoryMock.Verify(
     repo => repo.AddAsync(
  It.IsAny<User>(),
  cancellationToken),
     Times.Once);

 _unitOfWorkMock.Verify(
     uow => uow.SaveChangesAsync(cancellationToken),
     Times.Once);
    }
}
