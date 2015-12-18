using System;
using System.Web.Http;
//using System.Threading.Tasks;

using Owin;

using Microsoft.Owin;
using Microsoft.Owin.Cors;

using Newtonsoft.Json;

using C3PO.Services;
using C3PO.Services.Handlers;
using C3PO.Services.Controllers;

[assembly: OwinStartup(typeof(Startup))]

namespace C3PO.Services
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            //var name = typeof(BoardingController).FullName;

            var config = new HttpConfiguration();

            ////config.DependencyResolver = new UnityResolver(container);
            ////config.Services.Replace(typeof(IHttpControllerActivator), new UnityControllerActivator(container));

            //config.MapHttpAttributeRoutes(x => x.AddRoutesFromController<UserController>());
            //config.MapHttpAttributeRoutes(x => x.AddRoutesFromController<VesselController>());
            //config.MapHttpAttributeRoutes(x => x.AddRoutesFromController<PersonController>());
            //config.MapHttpAttributeRoutes(x => x.AddRoutesFromController<SearchController>());
            //config.MapHttpAttributeRoutes(x => x.AddRoutesFromController<BoardingController>());

            ////var sb = new StringBuilder();
            ////sb.AppendLine("Loaded routes:");
            ////config.Routes.ForEach(route =>
            ////    sb.AppendLine(string.Format("\t{0} : {1}",
            ////        route.RouteTemplate,
            ////        route.DataTokens.ContainsKey("httpMethods") ? string.Join(", ", (string[])route.DataTokens["httpMethods"]) : "")));
            ////_logger.Info(sb.ToString());

            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.MessageHandlers.Add(new CacheDisablingHandler());

            //config.MaxReceivedMessageSize = 20971520;

            //config.Routes.MapHttpRoute("ActionApi", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }
}
