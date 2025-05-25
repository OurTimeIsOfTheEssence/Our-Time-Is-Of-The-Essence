using dotenv.net; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace OurTime.WebUI.Data
{
    public class DesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Ladda .env
            DotEnv.Load();

            // Läs in appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Hämta conn-string med placeholder
            var connStr = config.GetConnectionString("DefaultConnection")!
                // Ersätt nu platshållare med riktiga värden
                .Replace("{AZURE_SQL_USER}", Environment.GetEnvironmentVariable("AZURE_SQL_USER")!)
                .Replace("{AZURE_SQL_PASSWORD}", Environment.GetEnvironmentVariable("AZURE_SQL_PASSWORD")!)
                .Replace("{AZURE_SQL_SERVER}", Environment.GetEnvironmentVariable("AZURE_SQL_SERVER")!)
                .Replace("{AZURE_SQL_DATABASE}", Environment.GetEnvironmentVariable("AZURE_SQL_DATABASE")!);

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(
                connStr,
                sqlOptions => sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
