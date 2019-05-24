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
        /// Allow all Cors policy.
        /// </summary>
        public const string CorsAllowAnyPolicy = "AllowAllCorsPolicy";

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
