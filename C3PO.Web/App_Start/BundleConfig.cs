using System.Web.Optimization;
using System.Web.Optimization.React;

namespace C3PO.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/main").Include(
                "~/Resources/scripts/app/xhr.min.js",
                "~/Resources/scripts/build/dist/clientBundle.js"
            ));

            ////// Force minification/combination even in debug mode
            ////BundleTable.EnableOptimizations = false;
            //bundles.Add(new BabelBundle("~/bundles/main").Include(
            //    // Add your JSX files here
            //    "~/Resources/scripts/src/C3PO.jsx",
            //    "~/Resources/scripts/src/home.jsx",
            //    "~/Resources/scripts/src/security.jsx"
            //    // You can include regular JavaScript files in the bundle too
            //    //"~/Content/ajax.js",
            //));
        }
    }
}
