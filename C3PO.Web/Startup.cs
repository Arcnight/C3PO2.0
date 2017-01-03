using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



using SimpleInjector;
using SimpleInjector.Integration.AspNetCore;
using SimpleInjector.Integration.AspNetCore.Mvc;

using JavaScriptViewEngine;

using C3PO.Data.Extensions;

using C3PO.Web.Security;
using C3PO.Web.Security.Providers;
using C3PO.Web.Security.Interfaces;
//using C3POSecurity = C3PO.Web.Security;

namespace C3PO.Web
{
    public class Startup
    {
        IHostingEnvironment _env;
        Container _iocContainer = new Container();

        //public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            /*
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables().Build();
            */
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJsEngine(builder =>
            {
                builder
                .UsePooledEngineFactory(poolOptions =>
                {
                    poolOptions.WatchPath = _env.WebRootPath;
                    poolOptions.WatchFiles = new List<string>
                    {
                        Path.Combine(poolOptions.WatchPath, "server.js")
                    };
                })
                .UseNodeRenderEngine(nodeOptions =>
                {
                    nodeOptions.ProjectDirectory = _env.WebRootPath;
                    nodeOptions.GetModuleName = (path, model, bag, values, area, type) => "server";
                });
            });

            //if (_env.IsDevelopment())
            //{
            //    services.AddNodeServices(options =>
            //    {
            //        options.LaunchWithDebugging = true;
            //        options.DebuggingPort = 5858;
            //    });
            //}

            double sessionLifetime = 5;

            //services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(_iocContainer));
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_iocContainer));
            services.AddSingleton<ISessionManager>(new SessionManager(sessionLifetime));
            //services.AddSingleton(new C3POSecurity.JwtFormat(sessionLifetime));

            services.AddAuthentication();

            // Add framework services.
            services.AddMvc();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSimpleInjectorAspNetRequestScoping(_iocContainer);

            _iocContainer.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();

            InitializeContainer(app);

            _iocContainer.Verify();

            app.UseOpenIdConnectServer((options) =>
            {
                options.Provider = new AuthorizationProvider();

                // Enable the authorization, logout, token and userinfo endpoints.
                options.TokenEndpointPath = "/api/account/login";
                options.LogoutEndpointPath = "/api/account/logout";
                options.UserinfoEndpointPath = "/api/account/userinfo";
                options.AuthorizationEndpointPath = string.Empty; // "/api/account/authorize";

                // Register a new ephemeral key, that is discarded when the application
                // shuts down. Tokens signed using this key are automatically invalidated.
                // This method should only be used during development.
                options.SigningCredentials.AddEphemeralKey();

                // On production, using a X.509 certificate stored in the machine store is recommended.
                // You can generate a self-signed certificate using Pluralsight's self-cert utility:
                // https://s3.amazonaws.com/pluralsight-free/keith-brown/samples/SelfCert.zip
                //
                // options.SigningCredentials.AddCertificate("7D2A741FE34CC2C7369237A5F2078988E17A6A75");
                //
                // Alternatively, you can also store the certificate as an embedded .pfx resource
                // directly in this assembly or in a file published alongside this project:
                //
                // options.SigningCredentials.AddCertificate(
                //     assembly: typeof(Startup).GetTypeInfo().Assembly,
                //     resource: "Mvc.Server.Certificate.pfx",
                //     password: "Owin.Security.OpenIdConnect.Server");

                // Note: see AuthorizationController.cs for more
                // information concerning ApplicationCanDisplayErrors.
                options.ApplicationCanDisplayErrors = true;
                options.AllowInsecureHttp = true;

                // Note: to override the default access token format and use JWT, assign AccessTokenHandler:
                // options.AccessTokenHandler = new JwtSecurityTokenHandler();
            });

            //// Set up authorization service and token generation
            //app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            //{
            //    AccessTokenFormat = jwtFormat,
            //    Provider = new AuthenticationProvider(),
            //    RefreshTokenProvider = new RefreshTokenProvider(),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(sessionTimeoutMin)
            //});

            //app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions()
            //{
            //    Provider = new SessionOAuthBearerProvider(),
            //    AuthenticationMode = AuthenticationMode.Active,
            //    AllowedAudiences = new[] { C3POSecurity.JwtFormat.Audiences },
            //    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
            //    {
            //        new SymmetricKeyIssuerSecurityTokenProvider(C3POSecurity.JwtFormat.Issuer, jwtFormat.SigningKey)
            //    }
            //});

            // ASP.NET default stuff here
            app.UseJsEngine(); // this needs to be before MVC

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Default}/{action=Index}/{id?}");
            });
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            _iocContainer.RegisterMvcControllers(app);
            _iocContainer.RegisterMvcViewComponents(app);
            _iocContainer.RegisterRespositories();

            // Add application services. For instance:
            //_iocContainer.Register<IUserRepository, SqlUserRepository>(Lifestyle.Scoped);

            // Cross-wire ASP.NET services (if any). For instance:
            //_iocContainer.RegisterSingleton(app.ApplicationServices.GetService<ILoggerFactory>());
            // NOTE: Prevent cross-wired instances as much as possible.
            // See: https://simpleinjector.org/blog/2016/07/
        }
    }
}
