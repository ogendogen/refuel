using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refuel.Models;

namespace Refuel.Areas.Panel.Pages.Refuels
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public InputRefuelModel Input { get; set; }
        public void OnGet()
        {
        }
    }
}
