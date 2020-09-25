using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft.Json.Linq;
using Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Net.Http;
using Refuel.APIModels;

namespace Refuel.APIControllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleAuthController : ControllerBase
    {
        private readonly ILogger<GoogleAuthController> _logger;
        private readonly IUsersManager _usersManager;

        public GoogleAuthController(ILogger<GoogleAuthController> logger, IUsersManager usersManager)
        {
            _logger = logger;
            _usersManager = usersManager;
        }

        [HttpPost("auth/google")]
        public async Task<GoogleAuthAPIResponse> GoogleLogin([FromBody] string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                return new GoogleAuthAPIResponse() { Status = "missing param" };
            }
            
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(new Uri($"https://oauth2.googleapis.com/tokeninfo?id_token={token}"));
            if (!response.IsSuccessStatusCode)
            {
                return new GoogleAuthAPIResponse() { Status = "error" };
            }

            var googleResponse = await response.Content.ReadAsStringAsync();
            JObject googleObject = JObject.Parse(googleResponse);

            if (googleObject.ContainsKey("email") && googleObject.ContainsKey("name"))
            {
                var user = await _usersManager.RegisterOrLoginGoogleUser(googleObject["name"].ToString(), googleObject["email"].ToString());

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true
                    });

                    
                return new GoogleAuthAPIResponse() { Status = "ok" };
            }

            return new GoogleAuthAPIResponse() { Status = "unknown" };
        }
    }
}
