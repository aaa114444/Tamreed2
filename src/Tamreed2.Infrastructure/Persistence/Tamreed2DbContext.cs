using Microsoft.EntityFrameworkCore;
using Tamreed2.Domain.Bookings;
using Tamreed2.Domain.Services;
using Tamreed2.Domain.Users;

namespace Tamreed2.Infrastructure.Persistence;

public sealed class Tamreed2DbContext : DbContext
{
    public Tamreed2DbContext(
        DbContextOptions<Tamreed2DbContext> options)
        : base(options)
    {
    }

    public DbSet<Booking> Bookings => Set<Booking>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(Tamreed2DbContext).Assembly);
    }
}
