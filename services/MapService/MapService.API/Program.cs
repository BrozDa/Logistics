using MapService.Application.SeedService;
using MapService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Serilog;

namespace MapService.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    "../logs/log.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Information()
                .CreateLogger();



            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.InstallDependencies(builder.Configuration);


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            using (var scope = app.Services.CreateScope())
            {
                
                var db = app.Services.GetRequiredService<MapServiceContext>();
                await db.Database.EnsureDeletedAsync();
                await db.Database.MigrateAsync();

                var seedData = Path.Combine(AppContext.BaseDirectory, "Data", "smallMap.json");
                Console.WriteLine(seedData);

                var seedService = app.Services.GetRequiredService<SeedService>();
                Console.WriteLine("Starting to seed data");
                await seedService.SeedInitialDataAsync(seedData);

            }

            //seeding
            

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
