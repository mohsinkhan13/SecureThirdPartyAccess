using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthEmbeddedMvc.IdentityServer
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
                    ClientName = "MVC Client",
                    ClientId = "mvc",
                    ClientSecrets = new List<Secret> {new Secret {Value= "csecret" } },
                    Flow = Flows.ClientCredentials,
                    RedirectUris = new List<string>
                    {
                        //"https://localhost:44363/"
                        "http://localhost:49868"
                    },

                    AllowAccessToAllScopes = true,
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:44319/"
                    }
                }
            };
        }
    }
}