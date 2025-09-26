﻿using Serilog;

namespace MapService.API.Installers
{
    public static class LoggerInstaller
    {
        public static void AddLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    "../logs/log.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Information()
                .CreateLogger();
        }
    }
}
