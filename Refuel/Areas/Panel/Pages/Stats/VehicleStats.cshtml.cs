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
    public class VehicleStatsModel : PageModel
    {
        private readonly IVehiclesManager _vehiclesManager;
        public VehicleStatsModel(IVehiclesManager vehiclesManager)
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

            ViewData["vehicleName"] = _vehiclesManager.GetVehicleManufacturerAndModelById(vehicleId);
            return Page();
        }
    }
}