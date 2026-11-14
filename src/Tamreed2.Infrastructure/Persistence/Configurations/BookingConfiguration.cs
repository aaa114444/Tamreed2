using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamreed2.Domain.Bookings;

namespace Tamreed2.Infrastructure.Persistence.Configurations;

public sealed class BookingConfiguration
    : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(booking => booking.Id);

        builder.Property(booking => booking.CustomerId)
            .IsRequired();

        builder.Property(booking => booking.ServiceId)
            .IsRequired();

        builder.Property(booking => booking.ScheduledAtUtc)
            .IsRequired();

        builder.Property(booking => booking.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(booking => booking.PriceAtBooking)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(booking => booking.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(booking => booking.CreatedAtUtc)
            .IsRequired();

        builder.OwnsOne(booking => booking.Address, address =>
        {
            address.Property(value => value.City)
                .HasColumnName("City")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(value => value.District)
                .HasColumnName("District")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(value => value.Street)
                .HasColumnName("Street")
                .HasMaxLength(200)
                .IsRequired();

            address.Property(value => value.BuildingNumber)
                .HasColumnName("BuildingNumber")
                .HasMaxLength(50)
                .IsRequired();

            address.Property(value => value.ApartmentNumber)
                .HasColumnName("ApartmentNumber")
                .HasMaxLength(50)
                .IsRequired(false);
        });

        builder.Navigation(booking => booking.Address)
            .IsRequired();
    }
}
