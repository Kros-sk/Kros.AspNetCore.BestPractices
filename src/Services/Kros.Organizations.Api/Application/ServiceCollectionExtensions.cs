﻿using FluentValidation.AspNetCore;
using Kros.KORM.Extensions.Asp;
using Kros.MediatR.Extensions;
using Kros.Organizations.Api.Application.Commands.Pipelines;
using Kros.Organizations.Api.Application.Queries.PipeLines;
using Kros.Organizations.Api.Infrastructure;
using Kros.Swagger.Extensions;
using Kros.ToDos.Base.Infrastructure;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
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
                o.DisableDataAnnotationsValidation = true;
            });

        /// <summary>
        /// Add KORM database.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="configuration">Configuration.</param>
        public static void AddKormDatabase(this IServiceCollection services, IConfiguration configuration)
            => services.AddKorm(configuration)
                .AddKormMigrations(o =>
                {
                    o.AddAssemblyScriptsProvider(Assembly.GetEntryAssembly(), "Kros.Organizations.Api.SqlScripts");
                })
                .UseDatabaseConfiguration<DatabaseConfiguration>()
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
            => services
                .AddSwaggerDocumentation(configuration, c =>
                {
                    c.EnableAnnotations();

                    string xmlDocFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Kros.Organizations.Api.xml");
                    if (File.Exists(xmlDocFilePath))
                    {
                        c.IncludeXmlComments(xmlDocFilePath);
                    }
                })
                .AddFluentValidationRulesToSwagger();

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
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole, PermissionsHelper.ClaimValues.OwnerRole,
                                                                                PermissionsHelper.ClaimValues.AdminRole);
                });

                options.AddPolicy(PoliciesHelper.WriterAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole, PermissionsHelper.ClaimValues.OwnerRole,
                                                                                PermissionsHelper.ClaimValues.AdminRole,
                                                                                PermissionsHelper.ClaimValues.WriterRole);
                });

                options.AddPolicy(PoliciesHelper.ReaderAuthPolicyName, policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add(scheme);
                    policyAdmin.RequireClaim(PermissionsHelper.Claims.UserRole, PermissionsHelper.ClaimValues.OwnerRole,
                                                                                PermissionsHelper.ClaimValues.AdminRole,
                                                                                PermissionsHelper.ClaimValues.WriterRole,
                                                                                PermissionsHelper.ClaimValues.ReaderRole);
                });
            });
        }
    }
}
