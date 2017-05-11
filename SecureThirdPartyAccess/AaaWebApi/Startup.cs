using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Thinktecture.IdentityServer.v3.AccessTokenValidation;
using System.Web.Http;

[assembly: OwinStartup(typeof(AaaWebApi.Startup))]

namespace AaaWebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var baseUri = "https://logintest.wolterskluwercloud.com/auth/core";

            app.UseIdentityServerJwt(new JwtTokenValidationOptions {
                Authority = baseUri,
            });

            // web api configuration
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}
