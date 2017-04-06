using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;

[assembly: OwinStartup(typeof(IdSrv.Console.Startup))]

namespace IdSrv.Console
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryUsers(Users.GetUsers())
                    .UseInMemoryClients(Clients.GetClients())
                    .UseInMemoryScopes(Scopes.GetScopes()),
                RequireSsl = false
            };

            app.UseIdentityServer(options);
        }
    }
}
