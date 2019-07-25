using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Kros.KORM.Extensions.Asp;
using System.Reflection;
using MediatR;
using Kros.MediatR.Extensions;
using System.IO;
using Swashbuckle.AspNetCore.Swagger;
using System;
using Kros.Swagger.Extensions;
using Kros.Organizations.Api.Application.Commands.Pipelines;
using Kros.Organizations.Api.Application.Queries.PipeLines;
using Kros.Organizations.Api.Infrastructure;

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
                    o.AddAssemblyScriptsProvider(Assembly.GetEntryAssembly(), "Kros.Organizations.Api.SqlScripts");
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
        /// <param name="services">DI container.</param>
        /// <param name="configuration">Application configuration.</param>
        /// </summary>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
            => services.AddSwaggerDocumentation(configuration, c => { c.AddFluentValidationRules(); });

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
                options.AddPolicy(PoliciesHelper.AdminAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole, PermissionsHelper.ClaimValues.AdminRole);
                });

                options.AddPolicy(PoliciesHelper.WriterAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole, PermissionsHelper.ClaimValues.AdminRole,
                                                                                PermissionsHelper.ClaimValues.WriterRole);
                });

                options.AddPolicy(PoliciesHelper.ReaderAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole, PermissionsHelper.ClaimValues.AdminRole,
                                                                                PermissionsHelper.ClaimValues.WriterRole,
                                                                                PermissionsHelper.ClaimValues.ReaderRole);
                });
            });
        }
    }
}
