using FluentValidation.AspNetCore;
using Kros.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.BuilderMiddlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Kros.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Kros.ToDos.Api
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
        public Startup(IHostingEnvironment env)
            : base(env)
        { }

        /// <summary>
        /// Configure IoC container.
        /// </summary>
        /// <param name="services">Service.</param>
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services
                .AddWebApi()
                .AddFluentValidation();

            services.AddApiJwtAuthentication(JwtAuthorizationHelper.JwtSchemeName, Configuration);

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseErrorHandling();
            app.UseAuthentication();
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
