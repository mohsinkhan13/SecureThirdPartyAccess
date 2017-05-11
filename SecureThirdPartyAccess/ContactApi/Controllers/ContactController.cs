using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using EUPaaS.SDK.Security.WebApi;
using SecureThirdPartyAccess.Model;
using System.Web;

namespace ContactApi.Controllers
{
    /// <summary>
    ///  The standard asp.net [Authorize] attribute just checks for any valid EupaaS.Auth ticket. 
    ///  To check the presence of specific scopes or claims its advised to use the csp 
    ///  ClaimsAuthorize attribute
    /// </summary>
    //[ClaimsAuthorize(ValidScopes="wksampleapi profile")]
    [Authorize()]
    public class ContactController : ApiController
    {
        public IEnumerable<Contact> Get() {
            var logonuser = HttpContext.Current.Request.LogonUserIdentity;
            //var claims = System.Security.Claims.ClaimsPrincipal.Current.Claims;
            //var claim = claims.FirstOrDefault(x => x.Type.ToLower() == "name");
            var listOfContacts = new List<Contact> {
                new Contact{FirstName = logonuser.Name}
            };
            return listOfContacts;
            //return new InMemoryContactsRepository().GetContacts();
        }
    }
}
