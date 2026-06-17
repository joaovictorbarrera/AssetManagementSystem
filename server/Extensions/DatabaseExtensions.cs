using Microsoft.EntityFrameworkCore;
using ThreatlockerAssetManagementSystem.Data;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        string? host = Environment.GetEnvironmentVariable("DB_HOST");
        string? port = Environment.GetEnvironmentVariable("DB_PORT");
        string? database = Environment.GetEnvironmentVariable("DB_NAME");
        string? username = Environment.GetEnvironmentVariable("DB_USER");
        string? password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        string connectionString = (!string.IsNullOrEmpty(host) &&
                                   !string.IsNullOrEmpty(port) &&
                                   !string.IsNullOrEmpty(database) &&
                                   !string.IsNullOrEmpty(username) &&
                                   !string.IsNullOrEmpty(password))
            ? $"Server=tcp:{host},{port};Initial Catalog={database};Persist Security Info=False;User ID={username};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
            : config.GetConnectionString("DefaultConnection")
              ?? throw new InvalidOperationException("No database connection string configured.");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        return services;
    }
}