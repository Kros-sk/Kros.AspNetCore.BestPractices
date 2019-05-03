using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace ApiGateway.Infrastructure
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
        /// Allow all Cors policy.
        /// </summary>
        public const string CorsAllowAnyPolicy = "AllowAllCorsPolicy";

        /// <summary>
        /// Http client name for communication with Identity Server.
        /// </summary>
        public const string IdentityServerHttpClientName = "IdentityServerClient";

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
                c.SwaggerDoc("v1", new Info { Title = "ApiGateway", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "ApiGateway.xml");

                if (File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }
            });
        }

        /// <summary>
        /// Add authorization.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="configuration">Configuration.</param>
        public static IServiceCollection AddAuthorization(
            this IServiceCollection services)
            => services.AddAuthorization(options =>
                {
                    options.AddPolicy("admin", policyAdmin =>
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
            return services;
        }

        /// <summary>
        /// Add Cors.
        /// </summary>
        /// <param name="services">DI container.</param>
        public static IServiceCollection AddCorsAllowAny(this IServiceCollection services)
            => services.AddCors(o => o.AddPolicy(CorsAllowAnyPolicy, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
    }
}
