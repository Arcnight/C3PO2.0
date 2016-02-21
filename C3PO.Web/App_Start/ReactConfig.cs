using React;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(C3PO.Web.ReactConfig), "Configure")]

namespace C3PO.Web
{
	public static class ReactConfig
	{
		public static void Configure()
		{
            //// If you want to use server-side rendering of React components, 
            //// add all the necessary JavaScript files here. This includes 
            //// your components as well as all of their dependencies.
            //// See http://reactjs.net/ for more information. Example:
            ////ReactSiteConfiguration.Configuration
            ////	.AddScript("~/Scripts/First.jsx")
            ////	.AddScript("~/Scripts/Second.jsx");

            //// If you use an external build too (for example, Babel, Webpack,
            //// Browserify or Gulp), you can improve performance by disabling 
            //// ReactJS.NET's version of Babel and loading the pre-transpiled 
            //// scripts. Example:
            ////ReactSiteConfiguration.Configuration
            ////	.SetLoadBabel(false)
            ////	.AddScriptWithoutTransform("~/Scripts/bundle.server.js")
            //ReactSiteConfiguration.Configuration.AddScript("~/Resources/scripts/views/C3PO.jsx")
            //                                    .AddScript("~/Resources/scripts/views/security.jsx")
            //                                    .AddScript("~/Resources/scripts/views/home.jsx");
            ReactSiteConfiguration.Configuration
                .AddScript("~/Resources/scripts/build/dist/serverBundle.js");
        }
    }
}