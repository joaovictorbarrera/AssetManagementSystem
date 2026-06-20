using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using ThreatlockerAssetManagementSystem.Data;
using ThreatlockerAssetManagementSystem.Models.Entities;
using ThreatlockerAssetManagementSystem.Repositories;

namespace ThreatlockerAssetManagementSystem.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger("Authentication");

            string jwtKey = config["JwtKey"] 
                                ?? throw new Exception("JwtKey missing from configuration.");

            if (config["TokenExpirationDays"] == null) 
                logger.LogWarning("TokenExpirationDays not configured. Defaulting to 7 days.");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey))
                    };

                    // On every request, we want to get the user for validation
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var userRepository =
                                context.HttpContext.RequestServices
                                    .GetRequiredService<UserRepository>();

                            Guid userId = context.Principal!.GetUserId();

                            User? user = await userRepository.GetUserByIdAsync(userId);

                            if (user == null || !user.IsActive)
                            {
                                context.Fail("User is inactive");
                                return;
                            }

                            // Create Claim for most current User Role 
                            var identity = (ClaimsIdentity) context.Principal!.Identity!;

                            identity.AddClaim(
                                new Claim(ClaimTypes.Role, user.Role.ToString()));

                            // Caching user object so endpoints can easily retrieve it
                            // without additional query
                            context.HttpContext.Items["User"] = user;
                        }
                    };
                });

            return services;
        }
    }
}
