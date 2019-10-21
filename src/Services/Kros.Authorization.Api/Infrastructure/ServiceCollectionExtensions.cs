using Kros.AspNetCore.Authorization;
using Kros.KORM.Extensions.Asp;
using Kros.Swagger.Extensions;
using Kros.ToDos.Base.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;
using System.Reflection;

namespace Kros.Authorization.Api.Extensions
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
                .InitDatabaseForIdGenerator()
                .AddKormMigrations(o =>
                {
                    o.AddAssemblyScriptsProvider(Assembly.GetEntryAssembly(), "Kros.Authorization.Api.Infrastructure.SqlScripts");
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
        /// <param name="configuration">Application configuration.</param>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
            => services.AddSwaggerDocumentation(configuration, c => { c.AddFluentValidationRules(); });

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

            return services.Scan(scan =>
                scan.FromCallingAssembly()
                    .AddClasses()
                    .AsMatchingInterface());
        }

        /// <summary>
        /// Configure api authorization.
        /// </summary>
        /// <param name="services">Collection of app services.</param>
        /// <param name="scheme">Scheme name for authentication.</param>
        /// <returns></returns>
        public static IServiceCollection AddApiJwtAuthorization(
            this IServiceCollection services,
            string scheme)
        {
            return services.AddAuthorization(options =>
            {
                options.AddPolicy(PoliciesHelper.OwnerAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole, PermissionsHelper.ClaimValues.OwnerRole);
                });

                options.AddPolicy(PoliciesHelper.AdminAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole, PermissionsHelper.ClaimValues.AdminRole,
                                                                                PermissionsHelper.ClaimValues.OwnerRole);
                });

                options.AddPolicy(PoliciesHelper.WriterAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole, PermissionsHelper.ClaimValues.AdminRole,
                                                                                PermissionsHelper.ClaimValues.OwnerRole,
                                                                                PermissionsHelper.ClaimValues.WriterRole);
                });

                options.AddPolicy(PoliciesHelper.ReaderAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole, PermissionsHelper.ClaimValues.AdminRole,
                                                                                PermissionsHelper.ClaimValues.OwnerRole,
                                                                                PermissionsHelper.ClaimValues.WriterRole,
                                                                                PermissionsHelper.ClaimValues.ReaderRole);
                });
            });
        }
    }
}