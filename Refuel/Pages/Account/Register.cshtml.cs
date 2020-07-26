using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Refuel.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public InputRegisterModel Input { get; set; }
        public void OnGet()
        {

        }
    }
}