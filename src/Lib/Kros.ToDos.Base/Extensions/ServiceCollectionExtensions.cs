using FluentValidation.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Swagger.Extensions;
using Kros.ToDos.Base.Infrastructure;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for registering services to the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register fluent validation.
        /// </summary>
        /// <param name="builder">MVC builder.</param>
        /// <returns>MVC builder.</returns>
        public static IMvcBuilder AddFluentValidation(this IMvcBuilder builder)
            => builder.AddFluentValidation(o =>
            {
                o.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                o.RegisterValidatorsFromAssembly(Assembly.GetEntryAssembly());
                o.DisableDataAnnotationsValidation = true;
            });

        /// <summary>
        /// Configure api authentication and authorization.
        /// </summary>
        /// <param name="services">Collection of app services.</param>
        /// <param name="scheme">Scheme name for authentication.</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationAndAuthorization(
            this IServiceCollection services,
            string scheme,
            IConfiguration configuration)
        {
            services.AddApiJwtAuthentication(scheme, configuration);
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
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole,
                        PermissionsHelper.ClaimValues.AdminRole,
                        PermissionsHelper.ClaimValues.OwnerRole);
                });

                options.AddPolicy(PoliciesHelper.WriterAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole,
                        PermissionsHelper.ClaimValues.AdminRole,
                        PermissionsHelper.ClaimValues.OwnerRole,
                        PermissionsHelper.ClaimValues.WriterRole);
                });

                options.AddPolicy(PoliciesHelper.ReaderAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole,
                        PermissionsHelper.ClaimValues.AdminRole,
                        PermissionsHelper.ClaimValues.OwnerRole,
                        PermissionsHelper.ClaimValues.WriterRole,
                        PermissionsHelper.ClaimValues.ReaderRole);
                });
            });
        }

        /// <summary>
        /// Add Swagger.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="configuration">Application configuration.</param>
        public static IServiceCollection AddSwagger(
            this IServiceCollection services,
            IConfiguration configuration,
            string xmlDocFilePath)
            => services
                .AddSwaggerDocumentation(configuration, c =>
                {
                    c.EnableAnnotations();
                    if (File.Exists(xmlDocFilePath))
                    {
                        c.IncludeXmlComments(xmlDocFilePath);
                    }
                })
                .AddFluentValidationRulesToSwagger();
    }
}
