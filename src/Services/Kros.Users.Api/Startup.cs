using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kros.Identity.Extensions;
using Kros.KORM.Extensions.Asp;
using Kros.MediatR.Extensions;
using Kros.Users.Api.Application.Model;
using Kros.Users.Api.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kros.Users.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.local.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServerAuthentication(Configuration);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policyAdmin =>
                {
                    policyAdmin.RequireClaim("isAdmin", "True");
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddKorm(Configuration)
               .InitDatabaseForIdGenerator()
               .AddKormMigrations(Configuration, o =>
               {
                   o.AddAssemblyScriptsProvider(Assembly.GetEntryAssembly(), "Kros.Users.Api.Infrastructure.SqlScripts");
               })
               .Migrate();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<UserRepository>();

            //services.AddMediatRNullCheckPostProcessor();
            services.Scan(scan =>
                scan.FromCallingAssembly()
                .AddClasses()
                .AsMatchingInterface());

            services.AddCors(o => o.AddPolicy("AllowAllCorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            }

            app.UseAuthentication();
            app.UseCors("AllowAllCorsPolicy");
            app.UseMiddleware<UserProfileMiddleware>(Options.Create(new IdentityServerOptions {
                AuthorityUrl = Configuration.GetSection("IdentityServerHandlers").Get<IList<IdentityServerOptions>>().First().AuthorityUrl
            }));

            app.UseHttpsRedirection();
            app.UseKormMigrations();
            app.UseMvc();
        }
    }
}
