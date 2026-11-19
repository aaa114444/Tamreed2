namespace Tamreed2.Contracts.Services;

public sealed record CreateServiceRequest(
    string Title,
    string Name,
    string Description,
    IReadOnlyList<string> Points,
    string TitleAr,
    string NameAr,
    string DescriptionAr,
    IReadOnlyList<string> PointsAr,
    ServiceDetailsContentRequest Details
);
