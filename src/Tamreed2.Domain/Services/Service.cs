using Tamreed2.Domain.Common.Exceptions;
using Tamreed2.Domain.Common.Models;

namespace Tamreed2.Domain.Services;

public sealed class Service : AggregateRoot
{
    public string Title { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public List<string> Points { get; private set; } = new();

    public string TitleAr { get; private set; } = null!;
    public string NameAr { get; private set; } = null!;
    public string DescriptionAr { get; private set; } = null!;
    public List<string> PointsAr { get; private set; } = new();
    public ServiceDetailsContent Details { get; private set; } = new();

    public ServiceStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }

    private Service() { }

    private Service(
        string title,
        string name,
        string description,
        IEnumerable<string>? points,
        string titleAr,
        string nameAr,
        string descriptionAr,
        IEnumerable<string>? pointsAr,
        ServiceDetailsContent? details = null)
    {
        UpdateDetails(
            title, name, description, points,
            titleAr, nameAr, descriptionAr, pointsAr, details);

        Status = ServiceStatus.Active;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public static Service Create(
        string title,
        string name,
        string description,
        IEnumerable<string>? points,
        string titleAr,
        string nameAr,
        string descriptionAr,
        IEnumerable<string>? pointsAr,
        ServiceDetailsContent? details = null)
    {
        return new Service(
            title, name, description, points,
            titleAr, nameAr, descriptionAr, pointsAr, details);
    }

    public void UpdateDetails(
        string title,
        string name,
        string description,
        IEnumerable<string>? points,
        string titleAr,
        string nameAr,
        string descriptionAr,
        IEnumerable<string>? pointsAr,
        ServiceDetailsContent? details = null)
    {
        if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(titleAr))
            throw new DomainException("Service title is required in at least one language.");

        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(nameAr))
            throw new DomainException("Service name is required in at least one language.");

        if (string.IsNullOrWhiteSpace(description) && string.IsNullOrWhiteSpace(descriptionAr))
            throw new DomainException("Service description is required in at least one language.");
Title = (title ?? string.Empty).Trim();
        Name = (name ?? string.Empty).Trim();
        Description = (description ?? string.Empty).Trim();
        Points = NormalizePoints(points);

        TitleAr = (titleAr ?? string.Empty).Trim();
        NameAr = (nameAr ?? string.Empty).Trim();
        DescriptionAr = (descriptionAr ?? string.Empty).Trim();
        PointsAr = NormalizePoints(pointsAr);
        Details = details ?? new ServiceDetailsContent();
    }

    private static List<string> NormalizePoints(IEnumerable<string>? points)
    {
        var normalized = (points ?? Enumerable.Empty<string>())
            .Select(point => point?.Trim() ?? string.Empty)
            .Where(point => !string.IsNullOrWhiteSpace(point))
            .ToList();

        if (normalized.Count > 20)
            throw new DomainException("A service cannot have more than 20 points.");

        if (normalized.Any(point => point.Length > 200))
            throw new DomainException("Each service point cannot exceed 200 characters.");

        return normalized;
    }

    public void Deactivate()
    {
        Status = ServiceStatus.Inactive;
    }

    public void Reactivate()
    {
        Status = ServiceStatus.Active;
    }
}

