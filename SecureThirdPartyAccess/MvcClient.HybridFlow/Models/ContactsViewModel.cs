using SecureThirdPartyAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcClient.HybridFlow.Models
{
    public class ContactsViewModel
    {
        public List<Contact> Contacts { get; set; }
        public List<SelectListItem> Companies { get; set; }
        public Dictionary<Guid, List<Tuple<Guid, string, string>>> CompanyUsers { get; set; }
    }
}