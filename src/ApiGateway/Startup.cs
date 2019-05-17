using ApiGateway.Infrastructure;
using Kros.Identity.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.BuilderMiddlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.JwtAuthorize;
using Ocelot.Middleware;

namespace ApiGateway
{
    /// <summary>
    /// Startup class.
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="env">Enviromnent variables.</param>
        /// <param name="env">App configuration.</param>
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddOcelotJwtAuthorize();

            services.AddIdentityServerAuthentication(_configuration);
            services.AddApplicationServices();
            services.AddWebApi();
            services.AddOcelot();
            services.AddSwaggerForOcelot(_configuration);
            services.AddCorsAllowAny();

            //services.AddTokenJwtAuthorize();
        }

        /// <summary>
        /// // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        public async void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseAuthorizationMiddleware(_configuration);
            app.UseCors(Infrastructure.ServiceCollectionExtensions.CorsAllowAnyPolicy);
            app.UseErrorHandling();
            app.UseMvc();

            app.UseSwaggerForOcelotUI(_configuration);
            await app.UseOcelot();
        }
    }
}
