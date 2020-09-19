using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Refuel.Areas.Panel.Pages.Refuels
{
    public class StatsModel : PageModel
    {
        private readonly IVehiclesManager _vehiclesManager;

        public StatsModel(IVehiclesManager vehiclesManager)
        {
            _vehiclesManager = vehiclesManager;
        }
        public IActionResult OnGet(int vehicleId)
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);
            if (!_vehiclesManager.IsUserOwnsVehicle(userId, vehicleId))
            {
                return Forbid();
            }

            return Page();
        }
    }
}
