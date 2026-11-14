using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamreed2.Domain.Services;

namespace Tamreed2.Infrastructure.Persistence.Configurations;

public sealed class ServiceConfiguration
    : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(service => service.Id);

        builder.Property(service => service.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(service => service.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(service => service.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(service => service.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(service => service.CreatedAtUtc)
            .IsRequired();
    }
}
