using AssetManagementSystem.Extensions;
using AssetManagementSystem.Models.Repositories;
using AssetManagementSystem.Repositories;
using AssetManagementSystem.Services;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
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
                .AddRoleAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Your-Token\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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
