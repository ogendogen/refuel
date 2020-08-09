using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Refuel.POCOs;
using Google.Apis.Auth.OAuth2.Requests;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Database.Models;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Database;

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
        public async Task GoogleLogin(string token)
        {
            string url = $"https://oauth2.googleapis.com/tokeninfo?id_token={token}";
            using (WebClient webClient = new WebClient())
            {
                string googleResponse = webClient.DownloadString(url);
                JObject googleObject = JObject.Parse(googleResponse);

            }
        }
    }
}
