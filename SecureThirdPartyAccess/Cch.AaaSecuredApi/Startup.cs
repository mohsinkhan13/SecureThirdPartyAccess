using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Thinktecture.IdentityServer.v3.AccessTokenValidation;

[assembly: OwinStartup(typeof(Cch.AaaSecuredApi.Startup))]

namespace Cch.AaaSecuredApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string baseURL = "https://logintest.wolterskluwercloud.com/auth/core";

            app.UseIdentityServerJwt(new JwtTokenValidationOptions
            {
                Authority = baseURL,
            });

            // web api configuration
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}
