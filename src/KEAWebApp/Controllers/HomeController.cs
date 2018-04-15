using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KEAWebApp.Models.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using KEAWebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace KEAWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly string idpClient = "https://rollcallkea-testidentity.azurewebsites.net/";        

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Hello()
        {
            await WriteOutIdentityInformation();

            var idp = new DiscoveryClient(idpClient);
            var metaDataResponse = await idp.GetAsync();
            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var response = await userInfoClient.GetAsync(accessToken);

            if (response.IsError)
            {
                throw new Exception(
                    "Problem accessing the UserInfo endpoint."
                    , response.Exception);
            }

            var userName = response.Claims.FirstOrDefault(t => t.Type == "name")?.Value;
            var userContact = response.Claims.ElementAt(3).ToString();

            WhoIsIt whoIsIt = new WhoIsIt() { Name = userName, Email = userContact };
            WhoIsItViewModel model = new WhoIsItViewModel(whoIsIt);

            return View(model);

        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        //Check debugger for more info
        public async Task WriteOutIdentityInformation()
        {
            
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);                        
            Debug.WriteLine($"Identity token: {identityToken}");                        
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
