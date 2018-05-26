using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Swashbuckle.AspNetCore.Swagger;
using WheelOfFate.Scheduling;

namespace WheelOfFate.Web
{
    public class Startup
    {
        private const string ApiTitle = "Wheel of Fate API";
        private const string ApiVersion = "v1";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddJsonOptions(options => 
                    {
                        options.SerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                    });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiVersion, new Info { Title = ApiTitle, Version = ApiVersion });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new SchedulingModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Program.IsDevelopment = env.IsDevelopment();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", String.Format("{0} {1}", ApiTitle, ApiVersion));
            });
            app.UseExceptionHandler("/Home/Error");      
            app.UseStaticFiles();         
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
