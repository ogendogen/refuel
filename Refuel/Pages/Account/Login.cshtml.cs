using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Refuel.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUsersManager usersManager;

        [BindProperty]
        public InputLoginModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public LoginModel(IUsersManager usersManager)
        {
            this.usersManager = usersManager;
        }

        public async Task OnGetAsync(string returnUrl = "")
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            returnUrl ??= Url.Content("~/");

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "")
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                User user = await usersManager.Authenticate(Input.Login, Input.Password);
                if (user == null)
                {
                    ModelState.AddModelError(String.Empty, "Niepoprawne dane logowania!");
                    return Page();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = Input.RememberMe
                    });

                if (!Url.IsLocalUrl(returnUrl))
                {
                    returnUrl = Url.Content("~/");
                }

                return LocalRedirect(returnUrl);
            }

            return Page();
        }
    }
}