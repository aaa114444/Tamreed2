using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamreed2.Domain.CallbackRequests;

namespace Tamreed2.Infrastructure.Persistence.Configurations;

public sealed class CallbackRequestConfiguration
    : IEntityTypeConfiguration<CallbackRequest>
{
    public void Configure(EntityTypeBuilder<CallbackRequest> builder)
    {
        builder.ToTable("CallbackRequests");

        builder.HasKey(request => request.Id);

        builder.Property(request => request.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(request => request.MobileNumber)
            .HasMaxLength(30)
            .IsRequired();

        var servicesProperty = builder.Property(request => request.ServicesNeeded);

        servicesProperty
            .HasConversion(
                services => JsonSerializer.Serialize(services, (JsonSerializerOptions?)null),
                json => string.IsNullOrWhiteSpace(json)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(
                        json, (JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        servicesProperty.Metadata.SetValueComparer(
            new ValueComparer<List<string>>(
                (left, right) => left != null && right != null
                    && left.SequenceEqual(right),
                services => services.Aggregate(
                    0, (hash, service) => HashCode.Combine(hash, service.GetHashCode())),
                services => services.ToList()));

        builder.Property(request => request.Message)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(request => request.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(request => request.CreatedAtUtc)
            .IsRequired();

        builder.Property(request => request.UpdatedAtUtc)
            .IsRequired(false);

        builder.HasIndex(request => request.CreatedAtUtc);
        builder.HasIndex(request => request.Status);
    }
}