using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cch.Apis
{
    public class ContactController : ApiController
    {
        // GET api/<controller>
        [Route("contact")]
        [Authorize]
        public async Task<IEnumerable<Contact>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.Claims;
            var scope = claims.ToList().FirstOrDefault(x => x.Type == "scope").Value;

            return Contacts.GetContacts(scope);
        }

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}

    }

    static class Contacts
    {
        private static List<Contact> _contacts = new List<Contact>{
                new Contact {ClientName="mohsin.khan",Name = "Contact 1" },
                new Contact {ClientName="mohsin.khan",Name = "Contact 2" },
                new Contact {ClientName="mohsin.khan",Name = "Contact 3" },
                new Contact {ClientName="mohsink13",Name = "Contact 4" },
                new Contact {ClientName="mohsink13",Name = "Contact 5" },
                new Contact {ClientName="mohsink13",Name = "Contact 6" }
            };

        public static List<Contact> GetContacts(string scope)
        {
            return _contacts.Where(x => x.ClientName == scope).ToList();
        }
    }

    public class Contact
    {
        public string ClientName { get; set; }
        public string Name { get; set; }
    }
}