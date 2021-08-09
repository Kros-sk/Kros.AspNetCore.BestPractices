using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Infrastructure;
using Kros.KORM.Extensions.Asp;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for registering services for this project to the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add KORM database.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="configuration">Configuration.</param>
        public static void AddKormDatabase(this IServiceCollection services, IConfiguration configuration)
            => services.AddKorm(configuration)
                .UseDatabaseConfiguration(new DatabaseConfiguration())
                .InitDatabaseForIdGenerators()
                .AddKormMigrations(o =>
                {
                    o.AddAssemblyScriptsProvider(Assembly.GetEntryAssembly(), "Kros.Authorization.Api.SqlScripts");
                })
                .Migrate();

        /// <summary>
        /// Add MediatR.
        /// </summary>
        /// <param name="services">DI container.</param>
        public static IServiceCollection AddMediatRDependencies(this IServiceCollection services)
            => services.AddMediatR(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Add application services and configure options.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="configuration">App configuration.</param>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.ConfigureOptions<JwtAuthorizationOptions>(configuration);
            services.ConfigureOptions<ApiJwtAuthorizationOptions>(configuration);

            services.AddTransient(s => s.GetService<IHttpContextAccessor>()?.HttpContext?.User);

            return services.Scan(scan =>
                scan.FromCallingAssembly()
                    .AddClasses()
                    .AsMatchingInterface());
        }
    }
}