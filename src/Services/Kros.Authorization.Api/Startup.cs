using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Extensions;
using Kros.Identity.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.BuilderMiddlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            services.AddIdentityServerAuthentication(Configuration);
            services.AddApiJwtAuthentication(JwtAuthorizationHelper.JwtSchemeName, Configuration);
            services.AddApiJwtAuthorization(JwtAuthorizationHelper.JwtSchemeName);

            services.AddWebApi();
            services.AddKormDatabase(Configuration);
            services.AddMediatRDependencies();
            services.AddApplicationServices(Configuration);
            services.AddSwagger();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization and User API V1");
            });
        }
    }
}
