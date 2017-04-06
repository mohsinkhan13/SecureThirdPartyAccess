using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace IdSrv.Console
{
    public static class Clients
    {
        public static List<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "crm",
                    ClientName = "crmApp",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("F621F470-9731-4A25-80EF-67A6F7C5F4B8".Sha256())
                    },
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedScopes = new List<string>
                    {
                        "mohsin.khan"
                    },
                    Flow = Flows.ClientCredentials,
                }
            };
        }
    }
}
