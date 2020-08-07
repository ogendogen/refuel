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

        public string FormError { get; set; }
        public string FormSuccess { get; set; }

        [BindProperty]
        public InputLoginModel Input { get; set; }

        public LoginModel(IUsersManager usersManager)
        {
            this.usersManager = usersManager;
        }

        public async Task OnGetAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                User user = await usersManager.Authenticate(Input.Login, Input.Password);
                if (user == null)
                {
                    FormError = "Niepoprawne dane logowania!";
                    return Page();
                }

                if (user.VerificationCode != "0")
                {
                    FormError = "Konto nie zostało jeszcze aktywowane!";
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

                FormSuccess = "Zostałeś poprawnie zalogowany!";

                return LocalRedirect("/Panel/Index");
            }

            return Page();
        }
    }
}