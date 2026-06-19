using Microsoft.EntityFrameworkCore;
using ThreatlockerAssetManagementSystem.Data;

namespace ThreatlockerAssetManagementSystem.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IHost app)
        {
            using var scope = app.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                .CreateLogger("Migrations");

            try
            {
                logger.LogInformation("Applying pending migrations...");
                db.Database.Migrate();
                logger.LogInformation("Database is up to date.");
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Migration failed. Application will not start.");
                throw;
            }
        }
    }
}
