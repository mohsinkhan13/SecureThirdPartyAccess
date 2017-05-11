using EUPaaS.SDK.Security.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AaaWebApi.Controllers
{
    public class HomeController : Controller
    {
        [ClaimsAuthorize(ValidScopes = "wksampleapi")]
        public dynamic Get() {
            var principal = User as ClaimsPrincipal;

            return from c in principal.Identities.First().Claims
                   select new {
                       c.Type,
                       c.Value
                   };
        }
    }
}
