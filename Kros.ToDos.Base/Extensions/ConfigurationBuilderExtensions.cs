using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using System;

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
        /// <param name="serviceName">Name of the service.</param>
        public static IConfigurationBuilder AddAzureAppConfiguration(
            this IConfigurationBuilder config,
            HostBuilderContext hostingContext,
            string serviceName)
        {
            var settings = config.Build();

            config.AddAzureAppConfiguration(options =>
            {
                var credential = hostingContext.HostingEnvironment.IsDevelopment()
                    ? new DefaultAzureCredential()
                    : (TokenCredential)new ManagedIdentityCredential();

                options
                    .Connect(new Uri(settings["AppConfig:Endpoint"]), credential)
                    .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()))
                    .Select("Base:*", hostingContext.HostingEnvironment.EnvironmentName)
                    .Select($"{serviceName}:*", hostingContext.HostingEnvironment.EnvironmentName)
                    .TrimKeyPrefix($"{serviceName}:")
                    .TrimKeyPrefix("Base:");
            });

            return config;
        }
    }
}
