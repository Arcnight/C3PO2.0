using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using JavaScriptViewEngine;

namespace C3PO.Web
{
    public class Startup
    {
        IHostingEnvironment _env;

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables().Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJsEngine(builder =>
            {
                builder.UsePooledEngineFactory(poolOptions =>
                {
                    poolOptions.WatchPath = Path.Combine(_env.WebRootPath, "dist");
                    poolOptions.WatchFiles = new List<string>
                    {
                        Path.Combine(poolOptions.WatchPath, "server.js")
                    };
                })
                .UseNodeRenderEngine(nodeOptions =>
                {
                    nodeOptions.ProjectDirectory = Path.Combine(_env.WebRootPath, "dist");
                    nodeOptions.GetModuleName = (path, model, bag, values, area, type) => "server";
                });
            });

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseJsEngine(); // this needs to be before MVC

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
