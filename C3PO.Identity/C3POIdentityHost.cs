using System;
using System.ServiceProcess;

using Microsoft.Owin.Hosting;

namespace C3PO.Identity
{
    public class C3POIdentityHost : ServiceBase
    {
        C3POIdentityHost() { }

        static void Main()
        {
            var service = new C3POIdentityHost();

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

            using (WebApp.Start<Startup>(url: "https://localhost:44333"))
            {
                Console.WriteLine("C3PO Identity Server Started...");
                Console.WriteLine(string.Empty);
                Console.Write("Hit any key to end...");

                Console.ReadKey();
            }
        }
    }
}