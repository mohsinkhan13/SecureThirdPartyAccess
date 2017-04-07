using System;
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
                new Contact {Practice="deloitte",FirstName = "Bob", LastName = "Cox" },
                new Contact {Practice="deloitte",FirstName = "Steve", LastName = "Green" },
                new Contact {Practice="deloitte",FirstName = "Joe", LastName = "Blogs" },
                new Contact {Practice="kpmg",FirstName = "Sarah", LastName="Smith" },
                new Contact {Practice="kpmg",FirstName = "Claire", LastName = "Shine" },
                new Contact {Practice="kpmg",FirstName = "Gemma", LastName = "G" }
            };

        public static List<Contact> GetContacts(string scope)
        {
            return _contacts.Where(x => x.Practice == scope).ToList();
        }
    }

    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Practice { get; set; }
    }
}