using MapService.Application.SeedService;
using MapService.Infrastructure.Persistence;
using MapService.Infrastructure.Repositories;
using MapService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MapService.API
{
    public static class Installer
    {
        public static IServiceCollection InstallDependencies (this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MapServiceContext>(
                    options => options.UseSqlServer(connectionString)
                );

            services.AddScoped<SeedService>();
            services.AddScoped<IMapRepository, MapRepository>();

            return services;
        }
    }
}
