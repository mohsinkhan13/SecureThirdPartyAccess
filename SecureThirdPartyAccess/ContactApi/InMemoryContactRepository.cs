using System.Collections.Generic;
using System.Linq;

namespace ContactApi
{
    public class InMemoryContactsRepository
    {
        private static List<Contact> _contacts = new List<Contact>{
                new Contact {Practice="deloitte",FirstName = "Bob", LastName = "Cox" },
                new Contact {Practice="deloitte",FirstName = "Steve", LastName = "Green" },
                new Contact {Practice="deloitte",FirstName = "Joe", LastName = "Blogs" },
                new Contact {Practice="kpmg",FirstName = "Sarah", LastName="Smith" },
                new Contact {Practice="kpmg",FirstName = "Claire", LastName = "Shine" },
                new Contact {Practice="kpmg",FirstName = "Gemma", LastName = "G" }
            };

        public List<Contact> GetContacts()
        {
            return _contacts.ToList();
        }
    }
}