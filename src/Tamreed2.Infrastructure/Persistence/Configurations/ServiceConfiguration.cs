using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamreed2.Domain.Services;

namespace Tamreed2.Infrastructure.Persistence.Configurations;

public sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");
        builder.HasKey(service => service.Id);

        builder.Property(service => service.Title)
            .HasMaxLength(200).IsRequired();

        builder.Property(service => service.Name)
            .HasMaxLength(150).IsRequired();

        builder.Property(service => service.Description)
            .HasMaxLength(1000).IsRequired();

        builder.Property(service => service.TitleAr)
            .HasMaxLength(200).IsRequired();

        builder.Property(service => service.NameAr)
            .HasMaxLength(150).IsRequired();

        builder.Property(service => service.DescriptionAr)
            .HasMaxLength(1000).IsRequired();

        ConfigurePoints(builder.Property(service => service.Points));
        ConfigurePoints(builder.Property(service => service.PointsAr));

        builder.Property(service => service.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(service => service.CreatedAtUtc)
            .IsRequired();
    }

    private static void ConfigurePoints(
        Microsoft.EntityFrameworkCore.Metadata.Builders.PropertyBuilder<List<string>> property)
    {
        property
            .HasConversion(
                points => JsonSerializer.Serialize(points, (JsonSerializerOptions?)null),
                json => string.IsNullOrWhiteSpace(json)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null)
                        ?? new List<string>())
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        property.Metadata.SetValueComparer(
            new ValueComparer<List<string>>(
                (left, right) => left != null && right != null && left.SequenceEqual(right),
                points => points.Aggregate(0, (hash, point) => HashCode.Combine(hash, point.GetHashCode())),
                points => points.ToList()));
    }
}

