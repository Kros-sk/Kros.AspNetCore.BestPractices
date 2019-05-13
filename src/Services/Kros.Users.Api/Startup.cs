using Kros.Users.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.BuilderMiddlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.JwtAuthorize;
using System.Linq;
using System.Security.Claims;

namespace Kros.Users.Api
{
    /// <summary>
    /// Startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Application configuration.
        /// </summary>
        public IConfiguration _configuration { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="env">Enviromnent variables.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.local.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiJwtAuthorize((context) =>
            {
                return ValidatePermission(context);
            });

            services.AddAppAuthorization();
            services.AddWebApi()
                .AddAuthorization();
            services.AddKormDatabase(_configuration);
            services.AddMediatRDependencies();
            services.AddCorsAllowAny();
            services.AddApplicationServices();
            services.AddSwagger();
        }

        /// <summary>
        /// Cusomer Validate Method
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private bool ValidatePermission(HttpContext httpContext)
        {
            if (httpContext.User.Claims.Count() > 0)
            {
                return true;
            } else
            {
                return false;
            }
            //var permissions = new List<Permission>() {
            //    new Permission { Name="admin", Predicate="Get", Url="/api/values" },
            //    new Permission { Name="admin", Predicate="Post", Url="/api/values" }
            //};
            //var questUrl = httpContext.Request.Path.Value.ToLower();

            //if (permissions != null && permissions.Where(w => w.Url.Contains("}") ? questUrl.Contains(w.Url.Split('{')[0]) : w.Url.ToLower() == questUrl && w.Predicate.ToLower() == httpContext.Request.Method.ToLower()).Count() > 0)
            //{
            //    var roles = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Role).Value;
            //    var roleArr = roles.Split(',');
            //    var perCount = permissions.Where(w => roleArr.Contains(w.Name)).Count();
            //    if (perCount == 0)
            //    {
            //        httpContext.Response.Headers.Add("error", "no permission");
            //        return false;
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}
            //else
            //{
            //    return false;
            //}
            //return true;
        }

        /// <summary>
        /// // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Enviromnent variables.</param>
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

            app.UseAuthentication();
            app.UseCors(Extensions.ServiceCollectionExtensions.CorsAllowAnyPolicy);
            app.UseErrorHandling();
            app.UseKormMigrations();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API V1");
            });
        }
    }
}
