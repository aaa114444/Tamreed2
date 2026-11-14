using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamreed2.Domain.Users;

namespace Tamreed2.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

 builder.HasKey(user => user.Id);

        builder.Property(user => user.Email)
     .IsRequired()
     .HasMaxLength(255);

        builder.HasIndex(user => user.Email)
     .IsUnique();

        builder.Property(user => user.PasswordHash)
     .IsRequired()
     .HasMaxLength(255);

        builder.Property(user => user.FirstName)
     .IsRequired()
     .HasMaxLength(100);

        builder.Property(user => user.LastName)
     .IsRequired()
     .HasMaxLength(100);

        builder.Property(user => user.Role)
     .HasConversion<string>()
     .HasMaxLength(20)
     .IsRequired();

        builder.Property(user => user.IsActive)
            .IsRequired();

        builder.Property(user => user.CreatedAtUtc)
     .IsRequired();

        builder.Property(user => user.LastLoginAtUtc)
     .IsRequired(false);
    }
}
