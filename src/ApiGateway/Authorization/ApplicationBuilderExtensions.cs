﻿using Kros.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;

namespace ApiGateway.Authorization
{
    /// <summary>
    /// Extensions for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add gateway authentication middleware to request pipeline.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> where middleware is added.</param>
        /// <param name="configuration">Configuration from which the options are loaded.
        /// Configuration must contains GatewayJwtAuthorization section.</param>
        /// <exception cref="InvalidOperationException">
        /// When <see cref="GatewayJwtAuthorizationOptions" /> section is missing in configuration.
        /// </exception>
        public static IApplicationBuilder UseGatewayJwtAuthorization_Custom(
            this IApplicationBuilder app,
            IConfiguration configuration)
        {
            GatewayJwtAuthorizationOptions option = configuration.GetSection<GatewayJwtAuthorizationOptions>();
            
            if (option is null)
            {
                throw new InvalidOperationException(
                    string.Format("'{0}' configuration section is missing or empty.",
                                  Helpers.GetSectionName<GatewayJwtAuthorizationOptions>()));
            }

            return app.UseMiddleware<GatewayAuthorizationMiddleware>(option);
        }

        /// <summary>
        /// Add gateway authentication middleware to request pipeline.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> where middleware is added.</param>
        /// <param name="configureOptions">Function for obtaining <see cref="GatewayJwtAuthorizationOptions"/>.</param>
        public static IApplicationBuilder UseGatewayJwtAuthorization_Custom(
            this IApplicationBuilder app,
            Func<GatewayJwtAuthorizationOptions> configureOptions)
            => app.UseMiddleware<GatewayAuthorizationMiddleware>(configureOptions());
    }
}
