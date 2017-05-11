using IdentityModel.Client;
using MvcClient.HybridFlow.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecureThirdPartyAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Http;

namespace MvcClient.HybridFlow.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public async Task<ActionResult> Contacts() {
            var idToken = System.Security.Claims.ClaimsPrincipal.Current.Claims.FirstOrDefault(ss => ss.Type == "id_token").Value;
            var accessToken = System.Security.Claims.ClaimsPrincipal.Current.Claims.FirstOrDefault(ss => ss.Type == "access_token").Value;

            var result = await CallApi(accessToken);
            ViewBag.Message = result.Content.ReadAsStringAsync().Result;

            var claims = System.Security.Claims.ClaimsPrincipal.Current.Claims;
            var claim = claims.FirstOrDefault(x => x.Type.ToLower() == "name");
            var listOfContacts = new List<Contact> {
                new Contact { FirstName = claim.Value }
            };

            var viewModel = await CallGraphApi();
            if (viewModel != null)
                viewModel.Contacts = listOfContacts;
            else
                return View("ErrorMsg");

            return View(viewModel);
        }

        public ActionResult Index() {
            
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        private async Task<System.Net.Http.HttpResponseMessage> CallApi(string accessToken)
        {
            var client = new System.Net.Http.HttpClient();
            client.SetBearerToken(accessToken);

            var result = await client.GetAsync("http://localhost:63055/api/contact");

            return result;
        }

        private async Task<ContactsViewModel> CallGraphApi() {
            var token = ((System.Security.Claims.ClaimsPrincipal)User).Identities.First().Claims.Single(x => x.Type == "access_token").Value;
            using (var client = new System.Net.Http.HttpClient()) {
                client.SetBearerToken(token);
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.WoltersKluwer.json");

                var graphApiServer = "https://apitest.wolterskluwercloud.com/graphapi/api/v1/";
                var response = await client.GetAsync(graphApiServer + "companies/?$select=CompanyId,DisplayName");
                var companies = new List<SelectListItem>();
                var companiesUsers = new Dictionary<Guid /* companyId */, List<Tuple<Guid /* UserId */, string /* user name */, string /* email */>>>();
                if (response.IsSuccessStatusCode) {
                    dynamic content = JObject.Parse(await response.Content.ReadAsStringAsync());

                    foreach (dynamic company in content._embedded.company) {
                        var companyId = company.CompanyId.Value;

                        companies.Add(new SelectListItem() { Value = companyId, Text = company.DisplayName.Value });
                        var users = new List<Tuple<Guid /* UserId */, string /* user name */, string /* email */>>();
                        var usersPageUrl =
                            string.Format(
                                "{0}companies/{1}/users?$expand=Identity($select=IdentityId,Name,Surname),Identity($expand=Emails)&$select=UserId",
                                graphApiServer, companyId);
                        do {
                            var respUsers = await client.GetAsync(usersPageUrl);
                            if (respUsers.IsSuccessStatusCode) {
                                {
                                    dynamic contUsers = JObject.Parse(await respUsers.Content.ReadAsStringAsync());
                                    foreach (dynamic user in contUsers._embedded.user) {
                                        if (!string.IsNullOrWhiteSpace(user._embedded.Identity.IdentityId.Value)) {
                                            var userId = Guid.Parse(user.UserId.Value);
                                            var userDisplayName = string.Format("{0} {1}", user._embedded.Identity.Name,
                                                user._embedded.Identity.Surname);
                                            var emails = "";
                                            foreach (var email in user._embedded.Identity._embedded.Emails) {
                                                emails += email.Email + " ";
                                            }
                                            users.Add(new Tuple<Guid, string, string>(userId, userDisplayName, emails));
                                        }
                                    }
                                    usersPageUrl = contUsers._links == null || contUsers._links.next == null || contUsers._links.next.href == null ? null : contUsers._links.next.href.Value;
                                }
                            }
                        } while (usersPageUrl != null);

                        companiesUsers.Add(Guid.Parse(companyId), users);
                    }
                }
                else {
                    ViewBag.StatusCode = response.StatusCode;
                    ViewBag.Content = await response.Content.ReadAsStringAsync();
                    return null;
                }

                var viewModel = new ContactsViewModel();
                viewModel.Companies = companies;
                viewModel.CompanyUsers = companiesUsers;

                client.DefaultRequestHeaders.Remove("Accept");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                return viewModel;
            }
        }
    }
}