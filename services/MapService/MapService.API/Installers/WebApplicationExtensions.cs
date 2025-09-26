using MapService.Application.SeedService;
using MapService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MapService.API.Installers
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> ApplyMigrationsAndSeedAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = app.Services.GetRequiredService<MapServiceContext>();
                await db.Database.EnsureDeletedAsync();
                await db.Database.MigrateAsync();

                var seeder = app.Services.GetRequiredService<SeedService>();
                var seedDataFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "smallMap.json");

                await seeder.SeedInitialDataAsync(seedDataFilePath);
            }
            return app;
        }

    }
    
}
