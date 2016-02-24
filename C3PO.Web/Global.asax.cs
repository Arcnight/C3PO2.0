using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Optimization;

namespace C3PO.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AttributeRoutingConfig.Start(); 
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ReactConfig.Configure();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
