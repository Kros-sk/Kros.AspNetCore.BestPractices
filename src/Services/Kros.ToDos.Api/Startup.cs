using System.Reflection;
using Kros.KORM.Extensions.Asp;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;

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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(o =>
                {
                    o.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                    o.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            services.AddKorm(Configuration)
                .InitDatabaseForIdGenerator()
                .AddKormMigrations(Configuration, o =>
                {
                    o.AddAssemblyScriptsProvider(Assembly.GetEntryAssembly(), "Kros.ToDos.Api.Infrastructure.SqlScripts");
                })
                .Migrate();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddPipelineBehaviorsForRequest<IUserResourceQuery, IUserResourceQueryResult>();
            services.AddPipelineBehaviorsForRequest<IUserResourceCommand>();

            services.AddMediatRNullCheckPostProcessor();
            services.Scan(scan =>
                scan.FromCallingAssembly()
                .AddClasses()
                .AsMatchingInterface());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ToDo API", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "Kros.ToDos.Api.xml");

                if (File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }
            });

            services.ConfigureOptions<DistributedCacheEntryOptions>(Configuration);
            if (Environment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                var redisOptions = Configuration.Get<RedisCacheOptions>();
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisOptions.Configuration;
                    options.InstanceName = redisOptions.InstanceName;
                });
            }
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
