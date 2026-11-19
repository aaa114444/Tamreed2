namespace Tamreed2.Contracts.Services;

public sealed record ServiceResponse(
    Guid Id,
    string Title,
    string Name,
    string Description,
    IReadOnlyList<string> Points,
    string TitleAr,
    string NameAr,
    string DescriptionAr,
    IReadOnlyList<string> PointsAr,
    ServiceDetailsContentResponse Details,
    string Status,
    DateTimeOffset CreatedAtUtc
);
