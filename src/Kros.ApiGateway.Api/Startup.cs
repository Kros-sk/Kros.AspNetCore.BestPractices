using Kros.AspNetCore.Authorization;
using Kros.AspNetCore.Extensions;
using Kros.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.BuilderMiddlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using MMLib.Ocelot.Provider.AppConfiguration;
using Kros.AspNetCore.ServiceDiscovery;

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
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
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
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// Configure IoC container.
        /// </summary>
        /// <param name="services">Service.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.SetProxy(Configuration);

            services.AddGatewayJwtAuthorization();
            services.AddControllers();
            services.AddOcelot()
                .AddAppConfiguration();
            services.AddSwaggerForOcelot(Configuration);
            services.AddAllowAnyOriginCors();
            services.AddApplicationInsightsTelemetry();
            services.AddServiceDiscovery();
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

            app.UseRouting();
            app.UseErrorHandling();
            app.UseAllowAllOriginsCors();
            app.UseGatewayJwtAuthorization(Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerForOcelotUI(Configuration);
            await app.UseOcelot();
        }
    }
}
