using IdentityModel.Client;
using Ids4_client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ids4_client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secured()
        {
            var idtoken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            var model = new SecureModel();
            model.Token = idtoken;

            model.Claims = User.Claims.ToList();

            await GetUserInfo();

            return View(model);
        }

        public async Task GetUserInfo()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                throw new Exception();
            }
            var idtoken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);


            var userinfo = await client.GetUserInfoAsync(new UserInfoRequest { Address = disco.UserInfoEndpoint, Token = idtoken });

            if (userinfo.IsError)
            {
                throw new Exception();
            }

            var claims = userinfo.Claims;

        }

        public async Task CallOauth()
        {


            //var tokenResponse = await client.RequestTokenAsync(new TokenRequest { Address = disco.TokenEndpoint, ClientId = "Client2", ClientSecret = "secret", GrantType="code" });

            //if (tokenResponse.IsError)
            //{
            //    throw new Exception();
            //}


            //var userinfo = await client.GetUserInfoAsync(new UserInfoRequest { Address = disco.UserInfoEndpoint, Token = tokenResponse.IdentityToken });
        }
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
