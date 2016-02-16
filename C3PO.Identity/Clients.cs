using System.Collections.Generic;

using IdentityServer3.Core.Models;

namespace C3PO.Identity
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "C3PO Services",
                    ClientId = "C3PO",
                    Flow = Flows.Implicit,

                    RedirectUris = new List<string>
                    {
                        "https://localhost:44333/"
                    },

                    AllowAccessToAllScopes = true
                }
            };
        }
    }
}
