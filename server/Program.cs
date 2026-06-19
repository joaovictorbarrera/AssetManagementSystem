using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ThreatlockerAssetManagementSystem.Data;
using ThreatlockerAssetManagementSystem.Extensions;
using ThreatlockerAssetManagementSystem.Models.Services;
using ThreatlockerAssetManagementSystem.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddScoped<UserRepository>()
                .AddScoped<TokenService>();

builder.Services.AddDatabase(builder.Configuration, builder.Environment)
                .AddCustomCors(builder.Environment)
                .AddJwtAuthentication(builder.Configuration)
                .AddRoleAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors();

app.ApplyMigrations();

app.Run();
