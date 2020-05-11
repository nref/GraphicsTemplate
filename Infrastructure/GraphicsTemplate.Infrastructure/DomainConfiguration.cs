using Microsoft.Extensions.Configuration;
using System;

namespace GraphicsTemplate.Infrastructure
{
    public class ConfigurationLoader
    {
        public static IConfiguration Load()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            return configuration;
        }
    }

    public class DomainConfiguration
    {
        public string GraphicsRoot { get; set; }
    }
}
