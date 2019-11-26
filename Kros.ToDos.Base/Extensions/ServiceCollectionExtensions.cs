using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for registering services to the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Attempts to set proxy to HttpClient.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="configuration">App configuration.</param>
        public static IServiceCollection SetProxy(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("Proxy");
            if (section.Exists())
            {
                HttpClient.DefaultProxy = section.Get<WebProxy>();
            }

            return services;
        }
    }
}