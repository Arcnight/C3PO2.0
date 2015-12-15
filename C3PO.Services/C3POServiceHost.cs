using System;
using System.ServiceProcess;

using Microsoft.Owin.Hosting;

namespace C3PO.Services
{
    public class C3POServiceHost : ServiceBase
    {
        C3POServiceHost() { }

        static void Main(string[] args)
        {
            var service = new C3POServiceHost();

            if (Environment.UserInteractive)
            {
                service.OnStart(null);
            }
            else
            {
                ServiceBase.Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            using (WebApp.Start<Startup>(url: "http://+:3030/"))
            {
                Console.WriteLine("C3PO Web Service Started...");
                Console.WriteLine(string.Empty);
                Console.Write("Hit any key to end...");

                Console.ReadKey();
            }
        }
    }
}