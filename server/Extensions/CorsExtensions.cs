namespace ThreatlockerAssetManagementSystem.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(
            this IServiceCollection services,
            IConfiguration config,
            IWebHostEnvironment env)
        {
            var logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger("Cors");

            string FrontendURL = config["FrontendURL"] ?? throw new Exception("FrontendURL missing from configuration.");

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(FrontendURL);

                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            logger.LogInformation($"Cors Enabled for {FrontendURL}");

            return services;
        }
    }
}
