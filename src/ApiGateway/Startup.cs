using ApiGateway.Infrastructure;
using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.BuilderMiddlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="env">Enviromnent variables.</param>
        /// <param name="configuration">App configuration.</param>
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Environment = Check.NotNull(env, nameof(env));
            Configuration = Check.NotNull(configuration, nameof(configuration));
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
            //base.ConfigureServices(services);

            services.AddGatewayJwtAuthorization();
            services.AddWebApi();
            services.AddOcelot();
            services.AddSwaggerForOcelot(Configuration);
            services.AddCorsAllowAny();
        }

        /// <summary>
        /// Configure web api pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public async void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
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
            app.UseCors(Infrastructure.ServiceCollectionExtensions.CorsAllowAnyPolicy);
            app.UseGatewayJwtAuthorization(Configuration);
            app.UseMvc();

            app.UseSwaggerForOcelotUI(Configuration);
            await app.UseOcelot();
        }
    }
}
