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
        public async Task<JsonResult> GoogleLogin(string token)
        {
            string url = $"https://oauth2.googleapis.com/tokeninfo?id_token={token}";
            JObject jObject = new JObject();

            using (WebClient webClient = new WebClient())
            {
                string googleResponse = webClient.DownloadString(url);
                JObject googleObject = JObject.Parse(googleResponse);

                if (googleObject.ContainsKey("error"))
                {
                    jObject.Add("status", "error");
                }
                else if (googleObject.ContainsKey("email") && googleObject.ContainsKey("name"))
                {
                    var user = await _usersManager.RegisterOrLoginGoogleUser(googleObject["name"].ToString(), googleObject["email"].ToString());

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Login),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true
                        });

                    
                    jObject.Add("status", "ok");
                }

                return new JsonResult(jObject);
            }
        }
    }
}
