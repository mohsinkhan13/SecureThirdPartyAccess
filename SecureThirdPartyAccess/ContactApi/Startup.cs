using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityServer.v3.AccessTokenValidation;

namespace ContactApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var baseUri = "https://logintest.wolterskluwercloud.com/auth/core";
            app.UseIdentityServerJwt(new JwtTokenValidationOptions
            {
                Authority = baseUri,
            });

            // web api configuration
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}