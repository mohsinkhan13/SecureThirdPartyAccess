using EUPaaS.SDK.Security.WebApi;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContactApi.Apis
{
    //[ClaimsAuthorize(ValidScopes = "wksampleapi")]
    [Authorize]
    public class ContactController : ApiController
    {
        [Route("contact")]
        public IEnumerable<Contact> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var scope = claims.ToList().FirstOrDefault(x => x.Type == "scope").Value;

            return new InMemoryContactsRepository().GetContacts(scope);
        }
    }
}