using Tamreed2.Domain.Common.Exceptions;
using Tamreed2.Domain.Users;
using Xunit;

namespace Tamreed2.UnitTests.Domain.Users;

public class UserTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateActiveUser()
    {
        // Arrange
        var email = "john.doe@example.com";
        var passwordHash = "$2a$12$eImiTXuWVxfaHNYY0iNDCew.kbP6jVD3qxvZKqaFGLZZ8WRmPb5dC";
        var firstName = "John";
        var lastName = "Doe";

        // Act
 var user = User.Create(email, passwordHash, firstName, lastName);

 // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("john.doe@example.com", user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
 Assert.Equal(UserRole.User, user.Role);
        Assert.True(user.IsActive);
        Assert.Null(user.LastLoginAtUtc);
    }

    [Fact]
    public void Create_WithValidDataAsAdmin_ShouldCreateAdminUser()
    {
        // Arrange
        var email = "admin@example.com";
 var passwordHash = "$2a$12$eImiTXuWVxfaHNYY0iNDCew.kbP6jVD3qxvZKqaFGLZZ8WRmPb5dC";

        // Act
 var user = User.Create(email, passwordHash, "Fatima", "Ahmed", UserRole.Admin);

        // Assert
        Assert.Equal(UserRole.Admin, user.Role);
    }

    [Fact]
    public void Create_WithEmptyEmail_ShouldThrowDomainException()
    {
 Assert.Throws<DomainException>(() =>
     User.Create("", "$2a$12$hash", "John", "Doe"));
    }

    [Fact]
    public void Create_WithInvalidEmail_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() =>
     User.Create("invalid-email", "$2a$12$hash", "John", "Doe"));
    }

    [Fact]
    public void Create_WithEmptyPasswordHash_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() =>
     User.Create("john@example.com", "", "John", "Doe"));
    }

    [Fact]
    public void Create_WithEmptyFirstName_ShouldThrowDomainException()
    {
 Assert.Throws<DomainException>(() =>
     User.Create("john@example.com", "$2a$12$hash", "", "Doe"));
    }

    [Fact]
    public void Create_WithEmptyLastName_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() =>
     User.Create("john@example.com", "$2a$12$hash", "John", ""));
    }

    [Fact]
    public void Create_ShouldNormalizeEmailToLowercase()
    {
        var user = User.Create("John.Doe@Example.COM", "$2a$12$hash", "John", "Doe");

        Assert.Equal("john.doe@example.com", user.Email);
    }

    [Fact]
    public void Create_ShouldTrimNames()
    {
        var user = User.Create("john@example.com", "$2a$12$hash", "  John  ", "  Doe  ");

        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
    }

    [Fact]
    public void UpdateProfile_WithValidData_ShouldUpdateFirstAndLastName()
    {
        var user = User.Create("john@example.com", "$2a$12$hash", "John", "Doe");

        user.UpdateProfile("Jane", "Smith");

 Assert.Equal("Jane", user.FirstName);
 Assert.Equal("Smith", user.LastName);
    }

    [Fact]
    public void UpdateProfile_WithEmptyFirstName_ShouldThrowDomainException()
    {
        var user = User.Create("john@example.com", "$2a$12$hash", "John", "Doe");

        Assert.Throws<DomainException>(() =>
            user.UpdateProfile("", "Smith"));
    }

    [Fact]
    public void UpdatePasswordHash_WithValidHash_ShouldUpdatePasswordHash()
    {
        var user = User.Create("john@example.com", "$2a$12$hash1", "John", "Doe");
        var newHash = "$2a$12$hash2";

        user.UpdatePasswordHash(newHash);

        Assert.Equal(newHash, user.PasswordHash);
    }

    [Fact]
    public void UpdatePasswordHash_WithEmptyHash_ShouldThrowDomainException()
    {
        var user = User.Create("john@example.com", "$2a$12$hash", "John", "Doe");

 Assert.Throws<DomainException>(() =>
     user.UpdatePasswordHash(""));
    }

    [Fact]
    public void RecordLogin_ShouldUpdateLastLoginAtUtc()
    {
        var user = User.Create("john@example.com", "$2a$12$hash", "John", "Doe");
        var beforeLogin = DateTimeOffset.UtcNow;

        user.RecordLogin();

        var afterLogin = DateTimeOffset.UtcNow;

        Assert.NotNull(user.LastLoginAtUtc);
 Assert.True(user.LastLoginAtUtc >= beforeLogin);
        Assert.True(user.LastLoginAtUtc <= afterLogin);
    }

    [Fact]
    public void Deactivate_WhenUserIsActive_ShouldDeactivateUser()
    {
        var user = User.Create("john@example.com", "$2a$12$hash", "John", "Doe");

        user.Deactivate();

        Assert.False(user.IsActive);
    }

    [Fact]
    public void Deactivate_WhenUserIsAlreadyInactive_ShouldThrowDomainException()
    {
 var user = User.Create("john@example.com", "$2a$12$hash", "John", "Doe");
 user.Deactivate();

        Assert.Throws<DomainException>(() =>
     user.Deactivate());
    }

    [Fact]
    public void Activate_WhenUserIsInactive_ShouldActivateUser()
    {
 var user = User.Create("john@example.com", "$2a$12$hash", "John", "Doe");
 user.Deactivate();

        user.Activate();

        Assert.True(user.IsActive);
    }

    [Fact]
    public void Activate_WhenUserIsAlreadyActive_ShouldThrowDomainException()
    {
 var user = User.Create("john@example.com", "$2a$12$hash", "John", "Doe");

        Assert.Throws<DomainException>(() =>
     user.Activate());
    }

    [Fact]
    public void GetFullName_ShouldReturnCombinedFirstAndLastName()
    {
 var user = User.Create("john@example.com", "$2a$12$hash", "John", "Doe");

        var fullName = user.GetFullName();

        Assert.Equal("John Doe", fullName);
    }
}
