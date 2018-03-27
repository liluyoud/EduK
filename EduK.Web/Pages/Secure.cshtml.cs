using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace EduK.Web.Pages
{
    [Authorize]
    public class SecureModel : PageModel
    {
        public string Cargo { get; set; }

        public async Task OnGetAsync()
        {
            //OpenIdConnectParameterNames.IdToken
            if (User.Identity.IsAuthenticated)
            {
                var discoveryClient = new DiscoveryClient("http://localhost:5000");
                var metaDataResponse = await discoveryClient.GetAsync();
                var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await userInfoClient.GetAsync(accessToken);
                var cargo = response.Claims.FirstOrDefault(c => c.Type == "cargo")?.Value;
                Cargo = cargo;
            }
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

    }
}