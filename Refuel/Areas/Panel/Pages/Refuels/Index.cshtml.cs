using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Refuel.Areas.Panel.Pages.Refuels
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IEnumerable<Vehicle> UserVehicles { get; set; }
        private readonly IVehiclesManager _vehiclesManager;

        public IndexModel(IVehiclesManager vehiclesManager)
        {
            _vehiclesManager = vehiclesManager;
        }

        public void OnGet()
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);
            UserVehicles = _vehiclesManager.GetAllUserVehicles(userId);
        }
    }
}
