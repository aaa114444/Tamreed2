using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tamreed2.Infrastructure.Persistence;

namespace Tamreed2.IntegrationTests;

public class TamreedApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<Tamreed2DbContext>>();
            services.RemoveAll<Tamreed2DbContext>();

            services.AddDbContext<Tamreed2DbContext>(options =>
                options.UseInMemoryDatabase("TamreedIntegrationTests"));
        });
    }
}
