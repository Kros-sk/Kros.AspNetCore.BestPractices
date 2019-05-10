using Kros.Identity.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Kros.AspNetCore.Extensions;

namespace ApiGateway.Infrastructure
{
    /// <summary>
    /// Extensions for application builder.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private const string IdentityServerHandlersConfigSectionKey = "IdentityServerHandlers";

        /// <summary>
        /// Use user profile middleware.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseUserProfileMiddleware(
            this IApplicationBuilder app,
            IConfiguration configuration)
            => app.UseMiddleware<UserProfileMiddleware>(
                configuration.GetSection<IList<IdentityServerOptions>>(IdentityServerHandlersConfigSectionKey).First(),
                configuration.GetSection<AppSettingsOptions>());
    }
}