using MapService.Application.SeedService;
using MapService.Infrastructure.Persistence;
using MapService.Infrastructure.Repositories.Interfaces;
using MapService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MapService.API.Installers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<SeedService>();
            services.AddScoped<IMapRepository, MapRepository>();

            return services;
        }
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MapServiceContext>(
                    options => options.UseSqlServer(connectionString)
                );

            return services;
        }
    }
}
