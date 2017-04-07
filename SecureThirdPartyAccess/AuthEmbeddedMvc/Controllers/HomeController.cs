using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            return View((User as ClaimsPrincipal).Claims);
        }

        public async Task<ActionResult> Contact()
        {
            

            var response = GetClientToken();
            var result = await CallApi(response);
            ViewBag.Message = result.Content.ReadAsStringAsync().Result;
            return View();
        }

        static TokenResponse GetClientToken()
        {
            var client = new TokenClient(
                "http://localhost:5000/connect/token",
                "crm",
                "F621F470-9731-4A25-80EF-67A6F7C5F4B8");

            return client.RequestClientCredentialsAsync("deloitte").Result;
        }

        static async Task<System.Net.Http.HttpResponseMessage> CallApi(TokenResponse response)
        {
            var client = new HttpClient();
            client.SetBearerToken(response.AccessToken);

            var result = await client.GetAsync("http://localhost:41360/contact");

            return result;
        }
    }
}