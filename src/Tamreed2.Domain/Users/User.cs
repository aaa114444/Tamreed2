using Tamreed2.Domain.Common.Exceptions;
using Tamreed2.Domain.Common.Models;

namespace Tamreed2.Domain.Users;

public sealed class User : AggregateRoot
{
    public string Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public UserRole Role { get; private set; }

    public bool IsActive { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset? LastLoginAtUtc { get; private set; }

    private User()
    {
    }

    private User(
        string email,
        string passwordHash,
 string firstName,
        string lastName,
        UserRole role)
    {
 if (string.IsNullOrWhiteSpace(email))
     throw new DomainException("Email is required.");

 if (!IsValidEmail(email))
     throw new DomainException("Email format is invalid.");

        if (string.IsNullOrWhiteSpace(passwordHash))
     throw new DomainException("Password hash is required.");

        if (string.IsNullOrWhiteSpace(firstName))
     throw new DomainException("First name is required.");

 if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required.");

        Email = email.ToLowerInvariant().Trim();
        PasswordHash = passwordHash;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Role = role;
        IsActive = true;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public static User Create(
 string email,
        string passwordHash,
        string firstName,
        string lastName,
        UserRole role = UserRole.User)
    {
        return new User(email, passwordHash, firstName, lastName, role);
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
     throw new DomainException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
     throw new DomainException("Last name is required.");

        FirstName = firstName.Trim();
 LastName = lastName.Trim();
    }

    public void UpdatePasswordHash(string newPasswordHash)
    {
 if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("Password hash is required.");

        PasswordHash = newPasswordHash;
    }

    public void RecordLogin()
    {
        LastLoginAtUtc = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
 if (!IsActive)
     throw new DomainException("User is already inactive.");

        IsActive = false;
    }

    public void Activate()
    {
 if (IsActive)
            throw new DomainException("User is already active.");

        IsActive = true;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
     return addr.Address == email;
        }
 catch
        {
     return false;
        }
    }

    public string GetFullName() => $"{FirstName} {LastName}";
}

