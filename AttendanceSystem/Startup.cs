using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystem.AppSettings;
using AttendanceSystem.Database;
using AttendanceSystem.Database.Configuration;
using AttendanceSystem.JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VueCliMiddleware;

namespace AttendanceSystem
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        [Obsolete]
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Content-Disposition"));
            });

            services.AddDbContext<AttendanceSystemDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddControllers();

            var connectionSetting = new ConnectionSetting(Configuration[AppSettingCodes.ConnectionString]);
            services.AddSingleton<IConnectionSetting>(connectionSetting); //for fetching connections from services or api
            services.AddSpaStaticFiles(options => options.RootPath = "client/dist");

            //register services
            var configureDependency = new ConfigureDependency();
            configureDependency.RegisterDependencies(services);
            ConfigureBaseSetting(services); //appsetting.json

            #region JWT
            var JWT = Configuration.GetSection("JWTConfig");
            Auth.JWTAuthentication(services, JWT);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();

            ExceptionHandlers.HandleExceptionTypes(app);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpaStaticFiles();
            app.UseStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "client";
                spa.Options.StartupTimeout = new TimeSpan(0, 5, 0);
                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve", port: 8080);
                }
            });
        }

        public void ConfigureBaseSetting(IServiceCollection services)
        {
            var baseSetting = new BaseSetting();
            baseSetting.ApplicationUrl = Configuration[AppSettingCodes.ApplicationUrl];
            services.AddSingleton<BaseSetting>(baseSetting);
        }
    }
}
