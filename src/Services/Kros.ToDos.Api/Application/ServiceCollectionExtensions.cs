using FluentValidation.AspNetCore;
using Kros.KORM.Extensions.Asp;
using Kros.MediatR.Extensions;
using Kros.Swagger.Extensions;
using Kros.ToDos.Api.Application;
using Kros.ToDos.Api.Application.Commands.PipeLines;
using Kros.ToDos.Api.Application.Queries.PipeLines;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for registering services for this project to the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register fluent validation.
        /// </summary>
        /// <param name="builder">MVC builder.</param>
        /// <returns>MVC builder.</returns>
        public static IMvcCoreBuilder AddFluentValidation(this IMvcCoreBuilder builder)
            => builder.AddFluentValidation(o =>
            {
                o.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                o.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

        /// <summary>
        /// Add KORM database.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="configuration">Configuration.</param>
        public static void AddKormDatabase(this IServiceCollection services, IConfiguration configuration)
            => services.AddKorm(configuration)
                .InitDatabaseForIdGenerator()
                .AddKormMigrations(configuration, o =>
                {
                    o.AddAssemblyScriptsProvider(Assembly.GetEntryAssembly(), "Kros.ToDos.Api.SqlScripts");
                })
                .Migrate();

        /// <summary>
        /// Add MediatR.
        /// </summary>
        /// <param name="services">DI container.</param>
        public static IServiceCollection AddMediatRDependencies(this IServiceCollection services)
            => services.AddMediatR(Assembly.GetExecutingAssembly())
                .AddPipelineBehaviorsForRequest<IUserResourceQuery>()
                .AddPipelineBehaviorsForRequest<IUserResourceCommand>()
                .AddMediatRNullCheckPostProcessor();

        /// <summary>
        /// Add Swagger.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="configuration">Application configuration.</param>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
            => services.AddSwaggerDocumentation(configuration, c => { c.AddFluentValidationRules(); });

        /// <summary>
        /// Add distributed cache.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="configuration">Configuration.</param>
        public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<DistributedCacheEntryOptions>(configuration);
            var redisOptions = configuration.GetSection<RedisCacheOptions>();

            if (redisOptions.UseRedis)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisOptions.ConnectionString;
                    options.InstanceName = redisOptions.InstanceName;
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            return services;
        }
    }
}
