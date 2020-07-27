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

        public string FormError { get; set; }
        public string FormSuccess { get; set; }

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
                    FormError = "Hasła różnią się od siebie!";
                    return Page();
                }

                if (await usersManager.IsEmailUsed(Input.Email))
                {
                    FormError = "Email jest już zajęty!";
                    return Page();
                }

                if (await usersManager.IsLoginUsed(Input.Login))
                {
                    FormError = "Login jest już zajęty!";
                    return Page();
                }

                User user = await usersManager.RegisterNewUser(Input.Login, Input.Password, Input.Email);
                if (user == null)
                {
                    FormError = "Błąd rejestracji!";
                    return Page();
                }

                usersManager.SaveChanges();
                FormSuccess = "Konto założone pomyślnie!";
            }

            return Page();
        }
    }
}