using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;

namespace Refuel.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUsersManager usersManager;

        [BindProperty]
        public InputRegisterModel Input { get; set; }
        public RegisterModel(IUsersManager usersManager)
        {
            this.usersManager = usersManager;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Input.Password != Input.Password2)
                {
                    ModelState.AddModelError(String.Empty, "Hasła różnią się od siebie!");
                    return Page();
                }

                if (await usersManager.IsEmailUsed(Input.Email))
                {
                    ModelState.AddModelError(String.Empty, "Email jest już zajęty!");
                    return Page();
                }

                if (await usersManager.IsLoginUsed(Input.Login))
                {
                    ModelState.AddModelError(String.Empty, "Login jest już zajęty!");
                    return Page();
                }

                User user = await usersManager.RegisterNewUser(Input.Login, Input.Password, Input.Email);
                if (user == null)
                {
                    ModelState.AddModelError(String.Empty, "Niepoprawne dane logowania!");
                    return Page();
                }

                usersManager.SaveChanges();
            }

            return Page();
        }
    }
}