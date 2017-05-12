using EUPaaS.SDK.Security;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel.Client;

[assembly: OwinStartupAttribute(typeof(MvcClient.HybridFlow.Startup))]
namespace MvcClient.HybridFlow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);

            string baseURL = "https://logintest.wolterskluwercloud.com/auth/core";

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            // Persists the authetication using a cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                CookieName = "DemoApp",
                //ExpireTimeSpan = TimeSpan.FromHours(10),
                //SlidingExpiration = false

            });

            string appClientId = "WK.UK.MvcClient";
            string appClientSecret = "123456";

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                // Main parameters
                Authority = baseURL,
                ClientId = appClientId,
                ResponseType = "code id_token token",
                Scope = "openid IDInfo wksampleapi profile offline_access WK.GraphAPI.User",
                RedirectUri = "http://localhost:18473/",
                PostLogoutRedirectUri = "http://localhost:18473/",
                SignInAsAuthenticationType = "Cookies",


                //UseTokenLifetime = false,

                Notifications = new OpenIdConnectAuthenticationNotifications {
                    // Authorization code received!!
                    AuthorizationCodeReceived = async n => {
                        // the authorization code was received, so now we call the Authorizatioin server to get
                        // the authorization and the refresh token 

                        // ----------------------------------------------------------------
                        // var result = await AuthUtil.ProcessAuthorizationCode(n, BaseURL,
                        //    "WK.UK.MvcClient", "123456");
                        // ----------------------------------------------------------------

                        // filter "protocol" claims
                        try {


                            List<Claim> claims = new List<Claim>(from c in n.AuthenticationTicket.Identity.Claims
                                                                 where c.Type != "iss" &&
                                                                       c.Type != "aud" &&
                                                                       c.Type != "nbf" &&
                                                                       c.Type != "exp" &&
                                                                       c.Type != "iat" &&
                                                                       c.Type != "nonce" &&
                                                                       c.Type != "c_hash" &&
                                                                       c.Type != "at_hash"
                                                                 select c);

                            // get access and refresh token
                            OAuth2Client tokenClient = new OAuth2Client(new Uri(baseURL + "/connect/token"), appClientId, appClientSecret);

                            Thinktecture.IdentityModel.Client.TokenResponse response = await tokenClient.RequestAuthorizationCodeAsync(n.Code, n.RedirectUri);


                            // get userinfo data
                            UserInfoClient userInfoClient = new UserInfoClient(new Uri(baseURL + "/connect/userinfo"), response.AccessToken);

                            UserInfoResponse userInfo = await userInfoClient.GetAsync();
                            userInfo.Claims.ToList().ForEach(ui => claims.Add(new Claim(ui.Item1, ui.Item2)));

                            claims.Add(new Claim("access_token", response.AccessToken));
                            claims.Add(new Claim("expires_at", DateTime.Now.AddSeconds(response.ExpiresIn).ToLocalTime().ToString()));
                            claims.Add(new Claim("refresh_token", response.RefreshToken));
                            claims.Add(new Claim("id_token", n.ProtocolMessage.IdToken));

                            // Authenticates the current application
                            n.AuthenticationTicket = new AuthenticationTicket(new ClaimsIdentity(claims.Distinct(new Thinktecture.IdentityModel.ClaimComparer()), n.AuthenticationTicket.Identity.AuthenticationType), n.AuthenticationTicket.Properties);
                        }
                        catch (Exception) {
                            throw;
                        }
                    }
                }
            });
        }
    }
}
