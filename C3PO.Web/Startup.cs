using System;

using System.Web.Http;
using System.Web.Http.Dispatcher;
//using System.Threading.Tasks;

using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using Microsoft.Practices.ServiceLocation;

using Owin;
using Newtonsoft.Json;

//using IdentityServer3.AccessTokenValidation;

using C3PO.Web;
using C3PO.Web.Handlers;
using C3PO.Web.Security;
using C3PO.Web.Controllers.Unity;

using C3POJWTFormat = C3PO.Web.Security.JwtFormat;

//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OpenIdConnect;

[assembly: OwinStartup(typeof(Startup))]

namespace C3PO.Web
{
    public class Startup
    {
        IServiceLocator _serviceLocator;

        public Startup()
        {
            _serviceLocator = InitializeServiceLocator();
        }

        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            //var name = typeof(BoardingController).FullName;

            var sessionTimeoutMin = 5; // ConfigHelper.GetValue<double>("authentication_webservice_access_token_timeout");

            var unityContainer = _serviceLocator.GetInstance<IUnityContainer>();
            unityContainer.RegisterType<SessionManager>(new InjectionConstructor(sessionTimeoutMin));
            unityContainer.RegisterInstance(new C3POJWTFormat(sessionTimeoutMin));

            var jwtFormat = _serviceLocator.GetInstance<C3POJWTFormat>();

            // Set up authorization service and token generation
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                AccessTokenFormat = jwtFormat,
                Provider = new AuthenticationProvider(),
                RefreshTokenProvider = new RefreshTokenProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(sessionTimeoutMin)
            });

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions()
            {
                Provider = new SessionOAuthBearerProvider(),
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { C3POJWTFormat.Audiences },
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                {
                    new SymmetricKeyIssuerSecurityTokenProvider(C3POJWTFormat.Issuer, jwtFormat.SigningKey)
                }
            });

            // Get your HttpConfiguration. In OWIN, you'll create one rather than using GlobalConfiguration.
            var config = new HttpConfiguration();

            var serializerSettings = config.Formatters.JsonFormatter.SerializerSettings;

            serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //// Need this to remove "k__backing" string that appears when objects marked as serializable are serialized as json
            //((DefaultContractResolver)serializerSettings.ContractResolver).IgnoreSerializableAttribute = true;

            config.DependencyResolver = new UnityResolver(unityContainer);
            config.Services.Replace(typeof(IHttpControllerActivator), new ControllerActivator(unityContainer));

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = "Cookies"
            //});
            //app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            //{
            //    Authority = "https://localhost:44333/identity",
            //    ClientId = "C3PO",
            //    ResponseType = "id_token",

            //    SignInAsAuthenticationType = "Cookies"
            //});

            //app.UseCors(CorsOptions.AllowAll);

            //var config = new HttpConfiguration();

            //////config.DependencyResolver = new UnityResolver(container);
            //////config.Services.Replace(typeof(IHttpControllerActivator), new UnityControllerActivator(container));

            ////config.MapHttpAttributeRoutes(x => x.AddRoutesFromController<UserController>());
            ////config.MapHttpAttributeRoutes(x => x.AddRoutesFromController<VesselController>());
            ////config.MapHttpAttributeRoutes(x => x.AddRoutesFromController<PersonController>());
            ////config.MapHttpAttributeRoutes(x => x.AddRoutesFromController<SearchController>());
            ////config.MapHttpAttributeRoutes(x => x.AddRoutesFromController<BoardingController>());

            //////var sb = new StringBuilder();
            //////sb.AppendLine("Loaded routes:");
            //////config.Routes.ForEach(route =>
            //////    sb.AppendLine(string.Format("\t{0} : {1}",
            //////        route.RouteTemplate,
            //////        route.DataTokens.ContainsKey("httpMethods") ? string.Join(", ", (string[])route.DataTokens["httpMethods"]) : "")));
            //////_logger.Info(sb.ToString());

            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.MessageHandlers.Add(new CacheDisablingHandler());

            //config.MaxReceivedMessageSize = 20971520;

            //config.Routes.MapHttpRoute("ActionApi", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            //config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            config.Filters.Add(new AuthorizeAttribute());

            app.UseWebApi(config);
        }

        IServiceLocator InitializeServiceLocator()
        {
            var unityContainer = new UnityContainer();
            unityContainer.LoadConfiguration();

            var locator = new UnityServiceLocator(unityContainer);

            unityContainer.RegisterInstance(typeof(IServiceLocator), locator, new ContainerControlledLifetimeManager());

            ServiceLocator.SetLocatorProvider(() => locator);

            return ServiceLocator.Current;
        }
    }
}
