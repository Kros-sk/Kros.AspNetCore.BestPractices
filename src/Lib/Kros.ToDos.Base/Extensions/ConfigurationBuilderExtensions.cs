using Azure.Core;
using Azure.Identity;
using Kros.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kros.ToDos.Base.Extensions
{
    /// <summary>
    /// <see cref="IConfigurationBuilder"/> extension
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds the azure application configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="hostingContext">The hosting context.</param>
        public static IConfigurationBuilder AddAzureAppConfiguration(
            this IConfigurationBuilder config,
            HostBuilderContext hostingContext)
        {
            var settings = config.Build();

            config.AddAzureAppConfiguration(options =>
            {
                var credential = new DefaultAzureCredential();

                options
                    .Connect(new Uri(settings["AppConfig:Endpoint"]), credential)
                    .ConfigureKeyVault(kv => kv.SetCredential(credential));

                IEnumerable<string> services = settings
                    .GetSection("AppConfig:Settings")
                    .AsEnumerable()
                    .Where(p => !p.Value.IsNullOrWhiteSpace())
                    .Select(p => p.Value);

                foreach (string service in services)
                {
                    options
                        .Select($"{service}:*", hostingContext.HostingEnvironment.EnvironmentName)
                        .TrimKeyPrefix($"{service}:");
                }
            });

            return config;
        }
    }
}
