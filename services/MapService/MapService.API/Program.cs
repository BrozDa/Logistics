using MapService.API.Installers;
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
            LoggerInstaller.AddLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();


            builder.Services.AddServices();
            builder.Services.AddPersistence(builder.Configuration);


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                await app.ApplyMigrationsAndSeedAsync();

            }
            app.MapControllers();

            app.Run();
        }
    }
}
