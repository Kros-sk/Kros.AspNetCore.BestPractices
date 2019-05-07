using Kros.Identity.Extensions;
using Kros.KORM.Extensions.Asp;
using Kros.Users.Api.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Kros.Users.Api.Extensions
{
    /// <summary>
    /// Extensions for registering services for this project to the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Claim type for admin user role.
        /// </summary>
        public const string ClaimTypeForAdmin = "IsAdmin";

        /// <summary>
        /// Http client name for communication with Identity Server.
        /// </summary>
        public const string IdentityServerHttpClientName = "IdentityServerClient";

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
                    o.AddAssemblyScriptsProvider(Assembly.GetEntryAssembly(), "Kros.Users.Api.Infrastructure.SqlScripts");
                })
                .Migrate();

        /// <summary>
        /// Add MediatR.
        /// </summary>
        /// <param name="services">DI container.</param>
        public static IServiceCollection AddMediatRDependencies(this IServiceCollection services)
            => services.AddMediatR(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Add Swagger.
        /// </summary>
        /// <param name="services">DI container.</param>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName); // https://wegotcode.com/microsoft/swagger-fix-for-dotnetcore/
            });

            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Users API", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "Kros.Users.Api.xml");

                if (File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }
                c.AddFluentValidationRules();
            });
        }

        /// <summary>
        /// Add Authentication and authorization.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="configuration">Configuration.</param>
        public static IServiceCollection AddAuthenticationAndAuthorization(
            this IServiceCollection services,
            IConfiguration configuration)
            => services.AddIdentityServerAuthentication(configuration)
                .AddAuthorization(options =>
                {
                    options.AddPolicy(Policies.Admin, policyAdmin =>
                    {
                        policyAdmin.RequireClaim(ClaimTypeForAdmin, "True");
                    });
                });

        /// <summary>
        /// Add application services.
        /// </summary>
        /// <param name="services">DI container.</param>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpClient(IdentityServerHttpClientName);

            //services.AddMediatRNullCheckPostProcessor();

            return services.Scan(scan =>
                scan.FromCallingAssembly()
                    .AddClasses()
                    .AsMatchingInterface());
        }
    }
}
