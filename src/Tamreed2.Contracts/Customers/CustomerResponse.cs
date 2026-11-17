namespace Tamreed2.Contracts.Customers;

public sealed record CustomerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? LastLoginAtUtc
);
