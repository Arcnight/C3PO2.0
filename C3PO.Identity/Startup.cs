using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using Owin;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Configuration;
//using IdentityServer3.Core.Services.InMemory;

namespace C3PO.Identity
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/identity", idSrvApp =>
            {
                idSrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "C3PO Identity Server",
                    SigningCertificate = GetSignedCertificate(),

                    Factory = new IdentityServerServiceFactory().
                          UseInMemoryClients(Clients.Get()).
                          UseInMemoryScopes(StandardScopes.All).
                          UseInMemoryUsers(Users.Get())
                });
            });
        }

        X509Certificate2 GetSignedCertificate()
        {
            return new X509Certificate2(@"C:\AppDev\C3PO2.0\C3PO.Services\certs\localhost.pfx", string.Empty);
        }
    }
}