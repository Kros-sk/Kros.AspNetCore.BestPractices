using FluentValidation.AspNetCore;
using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Application.Services;
using Kros.Identity.Extensions;
using Kros.Swagger.Extensions;
using Kros.ToDos.Base.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.BuilderMiddlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Kros.Authorization.Api
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

            services.AddIdentityServerAuthentication(Configuration);
            services.AddAuthenticationAndAuthorization(JwtAuthorizationHelper.JwtSchemeName, Configuration);

            services.AddControllers()
                .AddFluentValidation();

            services.AddKormDatabase(Configuration);
            services.AddMediatRDependencies();
            services.AddApplicationServices(Configuration);

            services.AddHttpClient<AuthorizationService>()
                .AddResiliencyDefaultPolicyHandler();

            services.AddSwagger(Configuration,
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Kros.Authorization.Api.xml"));
            services.AddApplicationInsightsTelemetry();
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
                app
                    .UseSwagger()
                    .UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("v1/swagger.json", "Authorization API V1");
                    });
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

            app.UseSwaggerDocumentation(Configuration);
        }
    }
}
