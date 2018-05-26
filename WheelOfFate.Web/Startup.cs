using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
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
    /// <summary>
    /// Startup for the ASP.NET Core web site.
    /// </summary>
    public class Startup
    {
        private const string ApiTitle = "Wheel of Fate API";
        private const string ApiVersion = "v1";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WheelOfFate.Web.Startup"/> class.
        /// </summary>
        /// <param name="env">Env.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration root.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Configures the services. This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddJsonOptions(options => 
                    {
                        options.SerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                    });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiVersion, 
                    new Info 
                    { 
                        Title = ApiTitle, 
                        Version = ApiVersion,
                        Description = "Randomly schedule engineers for support shifts based on given rules",
                        Contact = new Contact
                        {
                            Name = "Kevin Gorski",
                            Email = "kevin.gorski+wheeloffate@gmail.com",
                            Url = "https://github.com/kevingorski/WheelOfFate"
                        }
                    });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="builder">Builder.</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new SchedulingModule());
        }

        /// <summary>
        /// Configure the specified app, env and loggerFactory. This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">App.</param>
        /// <param name="env">Env.</param>
        /// <param name="loggerFactory">Logger factory.</param>
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
