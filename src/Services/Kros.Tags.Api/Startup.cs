using FluentValidation.AspNetCore;
using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Swagger.Extensions;
using Kros.Tags.Api.Application.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.BuilderMiddlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Kros.Tags.Api
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup : BaseStartup
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="env">Environment.</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
            : base(configuration, env)
        { }

        /// <summary>
        /// Configure IoC container.
        /// </summary>
        /// <param name="services">Service.</param>
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.SetProxy(Configuration);

            services
                .AddControllers()
                .AddFluentValidation();

            services.AddAuthenticationAndAuthorization(JwtAuthorizationHelper.JwtSchemeName, Configuration);

            services.AddKormDatabase(Configuration);
            services.AddMediatRDependencies();

            services.ConfigureOptions<AzureTableStorageSettings>(Configuration);
            services.Scan(scan =>
                scan.FromCallingAssembly()
                .AddClasses()
                .AsMatchingInterface());

            services.AddSwagger(Configuration,
                 Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Kros.Tags.Api.xml"));
        }

        /// <summary>
        /// Configure web api pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public override void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            base.Configure(app, loggerFactory);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation(Configuration);
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseErrorHandling();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseKormMigrations();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
