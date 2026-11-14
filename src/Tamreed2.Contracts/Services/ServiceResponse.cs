namespace Tamreed2.Contracts.Services;

public sealed record ServiceResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Status,
    DateTimeOffset CreatedAtUtc
);