//using Microsoft.Practices.ServiceLocation;
//using MPSLServiceLocator = Microsoft.Practices.ServiceLocation.ServiceLocator;

//using Microsoft.Practices.Unity;
//using Microsoft.Practices.Unity.Configuration;

namespace C3PO.Web.Utilities
{
    public static class IoC
    {
        static bool _serviceLocatorInitialized = false;

        //public static IServiceLocator ServiceLocator
        //{
        //    get
        //    {
        //        if (!_serviceLocatorInitialized)
        //        {
        //            var unityContainer = new UnityContainer();
        //            unityContainer.LoadConfiguration();

        //            var locator = new UnityServiceLocator(unityContainer);

        //            unityContainer.RegisterInstance(typeof(IServiceLocator), locator, new ContainerControlledLifetimeManager());

        //            MPSLServiceLocator.SetLocatorProvider(() => locator);

        //            _serviceLocatorInitialized = true;
        //        }

        //        return MPSLServiceLocator.Current;
        //    }
        //}
    }
}