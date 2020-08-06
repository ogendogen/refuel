using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Models;
using Utils;

namespace Refuel.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUsersManager _usersManager;
        private readonly IEmailManager _emailManager;
        private readonly IOptions<EmailSettings> _settings;

        public string FormError { get; set; }
        public string FormSuccess { get; set; }

        [BindProperty]
        public InputRegisterModel Input { get; set; }
        public RegisterModel(IUsersManager usersManager, IEmailManager emailManager, IOptions<EmailSettings> settings)
        {
            _usersManager = usersManager;
            _emailManager = emailManager;
            _settings = settings;
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

                if (await _usersManager.IsEmailUsed(Input.Email))
                {
                    FormError = "Email jest już zajęty!";
                    return Page();
                }

                if (await _usersManager.IsLoginUsed(Input.Login))
                {
                    FormError = "Login jest już zajęty!";
                    return Page();
                }

                User user = await _usersManager.RegisterNewUser(Input.Login, Input.Password, Input.Email);
                if (user == null)
                {
                    FormError = "Błąd rejestracji!";
                    return Page();
                }

                _emailManager.SendEmail(new Email()
                {
                    SmtpAddress = _settings.Value.PrimaryDomain,
                    Port = _settings.Value.PrimaryPort,
                    From = _settings.Value.UsernameEmail,
                    Password = "***",
                    To = user.Email,
                    Header = "Aktywacja konta Refuel",
                    Body = "testowa wiadomość"
                });

                _usersManager.SaveChanges();
                FormSuccess = "Konto założone pomyślnie! Aktywuj swoje konto klikając w link w swojej poczcie email.";
            }

            return Page();
        }
    }
}