using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Kros.AspNetCore.Extensions;

namespace ApiGateway
{
    /// <summary>
    /// Startup class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create web host builder.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddAzureAppConfiguration(hostingContext)
                        .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .AddJsonFile("ocelot.local.json", optional: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
