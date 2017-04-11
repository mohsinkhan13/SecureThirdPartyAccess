using IdentityModel.Client;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View((User as ClaimsPrincipal).Claims);
        }

        public async Task<ActionResult> Contact(string scope)
        {
            

            var response = GetClientToken(scope);
            var result = await CallApi(response);
            ViewBag.Message = result.Content.ReadAsStringAsync().Result;
            return View();
        }

        static TokenResponse GetClientToken(string scope)
        {
            var client = new TokenClient(
                "http://localhost:5000/connect/token",
                "crm",
                "F621F470-9731-4A25-80EF-67A6F7C5F4B8");

            return client.RequestClientCredentialsAsync(scope).Result;
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