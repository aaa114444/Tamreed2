using Moq;
using Tamreed2.Application.Auth.Commands.Login;
using Tamreed2.Application.Auth.Commands.Register;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Domain.Users;
using Xunit;

namespace Tamreed2.UnitTests.Application.Auth.Commands.Login;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAuthenticationService> _authenticationServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
 _userRepositoryMock = new Mock<IUserRepository>();
        _authenticationServiceMock = new Mock<IAuthenticationService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

 _handler = new LoginUserCommandHandler(
     _userRepositoryMock.Object,
     _authenticationServiceMock.Object,
     _tokenServiceMock.Object,
     _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnTokens()
    {
 // Arrange
        var command = new LoginUserCommand(
     "john@example.com",
     "Password123!");

 var user = User.Create(
     "john@example.com",
     "$2a$12$hashedPassword",
     "John",
     "Doe",
     UserRole.User);

 var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
 var refreshToken = "refresh_token_value";

 _userRepositoryMock
     .Setup(repo => repo.GetByEmailAsync(
  "john@example.com",
  It.IsAny<CancellationToken>()))
     .ReturnsAsync(user);

 _authenticationServiceMock
     .Setup(service => service.VerifyPassword(
  "Password123!",
  "$2a$12$hashedPassword"))
     .Returns(true);

 _unitOfWorkMock
     .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
     .ReturnsAsync(1);

 _tokenServiceMock
     .Setup(service => service.GenerateAccessToken(
  user.Id,
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
    }

    [Fact]
    public async Task Handle_WithNonExistentEmail_ShouldThrowInvalidOperationException()
    {
 // Arrange
        var command = new LoginUserCommand(
     "nonexistent@example.com",
     "Password123!");

 _userRepositoryMock
     .Setup(repo => repo.GetByEmailAsync(
  "nonexistent@example.com",
  It.IsAny<CancellationToken>()))
     .ReturnsAsync((User?)null);

 // Act & Assert
 var exception = await Assert.ThrowsAsync<InvalidOperationException>(
     () => _handler.Handle(command, CancellationToken.None));

 Assert.Contains("Invalid email or password", exception.Message);
    }

    [Fact]
    public async Task Handle_WithInactiveUser_ShouldThrowInvalidOperationException()
    {
 // Arrange
 var command = new LoginUserCommand(
     "john@example.com",
     "Password123!");

        var user = User.Create(
     "john@example.com",
     "$2a$12$hashedPassword",
     "John",
     "Doe");

 user.Deactivate();

 _userRepositoryMock
     .Setup(repo => repo.GetByEmailAsync(
  "john@example.com",
  It.IsAny<CancellationToken>()))
     .ReturnsAsync(user);

 // Act & Assert
 var exception = await Assert.ThrowsAsync<InvalidOperationException>(
     () => _handler.Handle(command, CancellationToken.None));

 Assert.Contains("inactive", exception.Message);
    }

    [Fact]
    public async Task Handle_WithWrongPassword_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new LoginUserCommand(
     "john@example.com",
     "WrongPassword123!");

 var user = User.Create(
     "john@example.com",
     "$2a$12$hashedPassword",
     "John",
     "Doe");

 _userRepositoryMock
     .Setup(repo => repo.GetByEmailAsync(
  "john@example.com",
  It.IsAny<CancellationToken>()))
     .ReturnsAsync(user);

 _authenticationServiceMock
     .Setup(service => service.VerifyPassword(
  "WrongPassword123!",
  "$2a$12$hashedPassword"))
     .Returns(false);

 // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
     () => _handler.Handle(command, CancellationToken.None));

 Assert.Contains("Invalid email or password", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldRecordLoginTime()
    {
        // Arrange
 var command = new LoginUserCommand(
     "john@example.com",
     "Password123!");

        var user = User.Create(
     "john@example.com",
     "$2a$12$hashedPassword",
     "John",
     "Doe");

 var beforeLogin = user.LastLoginAtUtc;

        _userRepositoryMock
     .Setup(repo => repo.GetByEmailAsync(
  "john@example.com",
  It.IsAny<CancellationToken>()))
     .ReturnsAsync(user);

        _authenticationServiceMock
     .Setup(service => service.VerifyPassword(
  It.IsAny<string>(),
  It.IsAny<string>()))
     .Returns(true);

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
        Assert.Null(beforeLogin);
 Assert.NotNull(user.LastLoginAtUtc);
    }
}
