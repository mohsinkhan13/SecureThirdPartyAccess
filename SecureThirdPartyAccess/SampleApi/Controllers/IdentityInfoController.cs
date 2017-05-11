using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using EUPaaS.SDK.Security.WebApi;

namespace SampleApi.Controllers
{
    /// <summary>
    ///  The standard asp.net [Authorize] attribute just checks for any valid EupaaS.Auth ticket. 
    ///  To check the presence of specific scopes or claims its advised to use the csp 
    ///  ClaimsAuthorize attribute
    /// </summary>
    [ClaimsAuthorize(ValidScopes="wksampleapi")]
    public class IdentityInfoController : ApiController
    {
        public dynamic Get()
        {
            var principal = User as ClaimsPrincipal;

            return from c in principal.Identities.First().Claims
                   select new
                   {
                       c.Type,
                       c.Value
                   };
        }
    }
}
