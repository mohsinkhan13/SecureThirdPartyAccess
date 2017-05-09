using IdentityModel.Client;
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
        public async Task<ActionResult> Index()
        {
            //var response = GetClientToken(scope);
            var idToken = System.Security.Claims.ClaimsPrincipal.Current.Claims.FirstOrDefault(ss => ss.Type == "id_token").Value;
            var accessToken = System.Security.Claims.ClaimsPrincipal.Current.Claims.FirstOrDefault(ss => ss.Type == "access_token").Value;

            var result = await CallApi(accessToken);
            ViewBag.Message = result.Content.ReadAsStringAsync().Result;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //static TokenResponse GetClientToken()
        //{
        //    var client = new TokenClient(
        //        "http://localhost:5000/connect/token",
        //        "WKSampleHybrid",
        //        "123456");

        //    return client.RequestAuthorizationCodeAsync().Result;
        //}

        static async Task<System.Net.Http.HttpResponseMessage> CallApi(string accessToken)
        {
            var client = new System.Net.Http.HttpClient();
            client.SetBearerToken(accessToken);

            var result = await client.GetAsync("http://localhost:47604/contact");

            return result;
        }
    }
}