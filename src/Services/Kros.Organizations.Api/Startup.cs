﻿using FluentValidation.AspNetCore;
using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Organizations.Api.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.BuilderMiddlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;

namespace Kros.Organizations.Api
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup : BaseStartup
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="env">Environment.</param>
        public Startup(IWebHostEnvironment env)
            : base(env)
        { }

        /// <summary>
        /// Configure IoC container.
        /// </summary>
        /// <param name="services">Service.</param>
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            HttpClient.DefaultProxy = new WebProxy(new Uri("http://192.168.1.3:3128"), true);

            services.ConfigureOptions<UserRoleOptions>(Configuration);

            services.AddControllers()
                .AddFluentValidation();

            services.AddApiJwtAuthentication(JwtAuthorizationHelper.JwtSchemeName, Configuration);
            services.AddApiJwtAuthorization(JwtAuthorizationHelper.JwtSchemeName);

            services.AddKormDatabase(Configuration);
            services.AddMediatRDependencies();

            services.AddHttpClient();

            services.Scan(scan =>
                scan.FromCallingAssembly()
                .AddClasses()
                .AsMatchingInterface());

            services.AddSwagger(Configuration);
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
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseErrorHandling();
            app.UseKormMigrations();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}
