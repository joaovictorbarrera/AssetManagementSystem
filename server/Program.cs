using Microsoft.EntityFrameworkCore;
using ThreatlockerAssetManagementSystem.Data;
using ThreatlockerAssetManagementSystem.Extensions;
using ThreatlockerAssetManagementSystem.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
    
builder.Services.AddCustomCors(builder.Environment);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

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

app.Run();
