using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;

namespace ApiGateway.Infrastructure
{
    /// <summary>
    /// Extensions for application builder.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(
            this IApplicationBuilder app,
            IConfiguration configuration)
            => app.UseMiddleware<AuthorizationMiddleware>(
                configuration.GetSection<JwtAuthorizationSecurityOptions>());

        public static IApplicationBuilder UseAuthorizationMiddleware(
            this IApplicationBuilder app,
            Func<JwtAuthorizationSecurityOptions> configureOptions)
            => app.UseMiddleware<AuthorizationMiddleware>(
                configureOptions.Invoke());
    }
}