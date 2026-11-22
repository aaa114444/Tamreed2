namespace Tamreed2.Contracts.CallbackRequests;

public sealed record CallbackRequestResponse(
    Guid Id,
    string Name,
    string MobileNumber,
    IReadOnlyList<string> ServicesNeeded,
    string? Message,
    string Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc
);
