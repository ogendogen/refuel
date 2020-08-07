using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Refuel.Pages.Account
{
    public class ActivateModel : PageModel
    {
        private readonly IUsersManager _usersManager;
        public string VerificationPassed { get; set; }
        public string VerificationFailed { get; set; }
        public ActivateModel(IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        public async Task<IActionResult> OnGet(int id, string activationCode)
        {
            int verified = await _usersManager.VerifyUser(id, activationCode);
            if (verified > 0)
            {
                VerificationPassed = "Twoje konto zosta³o zweryfikowane! Zostaniesz za chwilê przekierowany do formularza logowania!";
            }
            else
            {
                VerificationFailed = "Weryfikacja konta nie powiod³a siê! SprawdŸ swój link aktywacyjny";
            }

            Response.Headers.Add("REFRESH", "5;URL=/Account/Login");
            
            return Page();
        }
    }
}
