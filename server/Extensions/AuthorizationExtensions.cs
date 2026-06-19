namespace ThreatlockerAssetManagementSystem.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddRoleAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("AssetManagerOrHigher", policy =>
                {
                    policy.RequireAssertion(context => 
                        context.User.IsInRole("AssetManager") ||
                        context.User.IsInRole("Admin"));
                });

            return services;
        }
    }
}
