using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityServer.v3.AccessTokenValidation;

namespace SampleApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerJwt(new JwtTokenValidationOptions
            {
                Authority = Sample.Constants.IdentityServerBaseAddress,
            });

            // web api configuration
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}