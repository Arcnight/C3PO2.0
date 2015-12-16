using System;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace C3PO.Utilities
{
    /// <summary>
    /// Provides bootstrapping operations 
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Initializes and returns a component that implements the service locator interface.
        /// </summary>
        /// <remarks>
        /// The current implementation uses the Microsoft Unity framework to load an IoC container from app.config and
        /// creates a service locator that's tied to it.  Since this method loads the container each time it is called,
        /// a client should attempt to only call it once.
        /// </remarks>
        /// <returns></returns>
        public static IServiceLocator InitializeServiceLocator()
        {
            var unityContainer = new UnityContainer().LoadConfiguration();
            var locator = new UnityServiceLocator(unityContainer);

            unityContainer.RegisterInstance(typeof(IServiceLocator), locator, new ContainerControlledLifetimeManager());

            ServiceLocator.SetLocatorProvider(() => locator);

            return ServiceLocator.Current;
        }
    }
}
