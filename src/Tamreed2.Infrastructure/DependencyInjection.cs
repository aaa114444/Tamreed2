using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tamreed2.Application.Common.Interfaces;
using Tamreed2.Infrastructure.Persistence;
using Tamreed2.Infrastructure.Persistence.Repositories;
using Tamreed2.Infrastructure.Authentication;

namespace Tamreed2.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found.");
        }

        services.AddDbContext<Tamreed2DbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Authentication services
        services.AddScoped<IAuthenticationService, PasswordHashingService>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}