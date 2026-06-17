namespace ThreatlockerAssetManagementSystem.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    if (env.IsDevelopment())
                    {
                        Console.WriteLine("Cors set to localhost:4200");
                        policy.WithOrigins("http://localhost:4200");
                    }
                    else
                    {
                        Console.WriteLine("Cors set to prod URL");
                        policy.WithOrigins("https://threatlocker-asset-management.vercel.app/");
                              
                    }

                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            return services;
        }
    }
}
