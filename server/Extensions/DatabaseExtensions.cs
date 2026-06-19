using Microsoft.EntityFrameworkCore;
using ThreatlockerAssetManagementSystem.Data;

namespace ThreatlockerAssetManagementSystem.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration config,
            IWebHostEnvironment env)
        {
            string? host = config["DB_HOST"];
            string? port = config["DB_PORT"];
            string? database = config["DB_NAME"];
            string? username = config["DB_USER"];
            string? password = config["DB_PASSWORD"];

            string connectionString;

            bool hasEnvVars =
                !string.IsNullOrWhiteSpace(host) &&
                !string.IsNullOrWhiteSpace(port) &&
                !string.IsNullOrWhiteSpace(database) &&
                !string.IsNullOrWhiteSpace(username) &&
                !string.IsNullOrWhiteSpace(password);

            if (hasEnvVars)
            {
                connectionString =
                    $"Server=tcp:{host},{port};" +
                    $"Initial Catalog={database};" +
                    $"Persist Security Info=False;" +
                    $"User ID={username};Password={password};" +
                    $"MultipleActiveResultSets=False;" +
                    $"Encrypt=True;TrustServerCertificate=False;" +
                    $"Connection Timeout=30;";
            }
            else if (env.IsDevelopment())
            {
                connectionString = config.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Missing DefaultConnection in appsettings.Development.json");
            }
            else
            {
                throw new InvalidOperationException(
                    "Production environment requires DB_HOST/DB_PORT/DB_NAME/DB_USER/DB_PASSWORD environment variables");
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
            });

            return services;
        }
    }
}