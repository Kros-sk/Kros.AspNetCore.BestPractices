﻿using System.Reflection;
using Kros.KORM.Extensions.Asp;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kros.ToDos.Api.Application.Queries.PipeLines;
using Kros.AspNetCore.Middlewares;
using Kros.MediatR.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System;
using FluentValidation.AspNetCore;
using Kros.ToDos.Api.Application.Commands.PipeLines;
using Microsoft.Extensions.Caching.Distributed;
using Kros.ToDos.Api.Infrastructure;

namespace Kros.ToDos.Api
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="env">Environment.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.local.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Environment = env;
        }

        /// <summary>
        /// Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Environment.
        /// </summary>
        public IHostingEnvironment Environment { get; }

        /// <summary>
        /// Configure IoC container.
        /// </summary>
        /// <param name="services">Service.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebApi()
                .AddFluentValidation();

            services.AddKormDatabase(Configuration);
            services.AddMediatRDependencies();

            services.Scan(scan =>
                scan.FromCallingAssembly()
                .AddClasses()
                .AsMatchingInterface());

            services.AddSwagger();
            services.AddDistributedCache(Configuration);
        }

        /// <summary>
        /// configure web api pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseErrorHandling();
            app.UseKormMigrations();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDos API V1");
            });
        }
    }
}
