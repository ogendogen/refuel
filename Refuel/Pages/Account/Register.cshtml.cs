using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json.Linq;
using Utils;

namespace Refuel.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUsersManager _usersManager;
        private readonly IEmailManager _emailManager;
        private readonly IOptions<EmailSettings> _settings;
        private readonly IOptions<Passwords> _passwords;
        private readonly IOptions<Recaptcha> _recaptcha;

        public string FormError { get; set; }
        public string FormSuccess { get; set; }
        public string RecaptchaSiteKey { get; set; }

        [BindProperty]
        public InputRegisterModel Input { get; set; }
        public RegisterModel(IUsersManager usersManager, 
            IEmailManager emailManager, 
            IOptions<EmailSettings> settings,
            IOptions<Passwords> passwords,
            IOptions<Recaptcha> recaptcha)
        {
            _usersManager = usersManager;
            _emailManager = emailManager;
            _settings = settings;
            _passwords = passwords;
            _recaptcha = recaptcha;

            RecaptchaSiteKey = _recaptcha.Value.SiteKey;
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

                if (!IsReCaptchaValid())
                {
                    FormError = "Nieudana weryfikacja ReCaptcha! Spróbuj ponownie.";
                    return Page();
                }

                string code = Utils.Utils.UrlEncode(Utils.Utils.ToBase64(user.VerificationCode));

                _usersManager.SaveChanges();

                _emailManager.SendEmail(new Email()
                {
                    SmtpAddress = _settings.Value.PrimaryDomain,
                    Port = _settings.Value.PrimaryPort,
                    From = _settings.Value.UsernameEmail,
                    Password = _passwords.Value.EmailPassword,
                    To = user.Email,
                    Header = "Aktywacja konta Refuel",
                    Body = $"Witaj {user.Login}! Kliknij aby aktywować konto {Request.Scheme}://{Request.Host}{Request.PathBase}/Account/Activate?id={user.ID}&activationCode={code}",
                });

                FormSuccess = "Konto założone pomyślnie! Aktywuj swoje konto klikając w link w swojej poczcie email.";
            }

            return Page();
        }

        public bool IsReCaptchaValid()  
        {
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = _recaptcha.Value.SiteKey;
            var apiUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}";
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
      
            using(WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    return jResponse.Value<bool>("success");
                }
            }
        }
    }
}