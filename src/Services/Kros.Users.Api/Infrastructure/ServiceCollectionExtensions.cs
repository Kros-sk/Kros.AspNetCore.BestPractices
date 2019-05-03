using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Kros.Users.Api.Infrastructure
{
    /// <summary>
    /// Extensions for registering services for this project to the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add authorization.
        /// </summary>
        /// <param name="services">DI container.</param>
        public static IServiceCollection AddAppAuthorization(
            this IServiceCollection services)
        {
            services.AddAuthentication(CustomAuthenticationHandler.CustomAuthenticationSchemaName)
                .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>
                (CustomAuthenticationHandler.CustomAuthenticationSchemaName, null);
            return services.AddAuthorization(options =>
                {
                    options.AddPolicy(CustomAuthenticationHandler.CustomAuthorizationAdminPolicyName, policyAdmin =>
                    {
                        policyAdmin.RequireClaim(CustomAuthenticationHandler.ClaimTypeForAdmin, 
                            CustomAuthenticationHandler.CustomAuthorizationAdminPolicyValue);
                    });
                });
        }
    }
}
