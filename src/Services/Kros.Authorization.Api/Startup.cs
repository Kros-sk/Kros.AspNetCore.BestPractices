using Kros.AspNetCore.Middlewares;
using Kros.Authorization.Api.Extensions;
using Kros.Identity.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;

namespace Kros.Authorization.Api
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
            services.AddIdentityServerAuthentication(_configuration);
            services.AddAuthentication("bla")
            .AddJwtBearer("bla", x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("XL3TcKtY5y7i6DR6wYkAp3EKBoZNke05PSMSs5Enrzun2bjmZsM")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policyAdmin =>
                {
                    policyAdmin.AuthenticationSchemes.Add("bla");
                    policyAdmin.RequireClaim("IsAdmin", "True");
                });
            });
            //services.AddAppAuthorization();
            services.AddWebApi()
                .AddAuthorization();
            services.AddKormDatabase(_configuration);
            services.AddMediatRDependencies();
            services.AddCorsAllowAny();
            services.AddApplicationServices(_configuration);
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
            }
            else
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
