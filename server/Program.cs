using AssetManagementSystem.Extensions;
using AssetManagementSystem.Models.Repositories;
using AssetManagementSystem.Repositories;
using AssetManagementSystem.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false)
        );
    });

builder.Services.AddScoped<UserRepository>()
                .AddScoped<AssetRepository>()
                .AddScoped<CheckoutRequestRepository>()
                .AddScoped<TokenService>()
                .AddScoped<CheckoutRequestService>()
                .AddScoped<AssetService>();

builder.Services.AddDatabase(builder.Configuration, builder.Environment)
                .AddCustomCors(builder.Configuration, builder.Environment)
                .AddJwtAuthentication(builder.Configuration)
                .AddRoleAuthorization()
                .AddSwagger();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.ApplyMigrations();

app.Run();
