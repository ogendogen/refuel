using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Refuel.Areas.Panel.Pages.Vehicles
{
    public class DetailsModel : PageModel
    {
        private readonly IVehiclesManager _vehiclesManager;

        public DetailsModel(IVehiclesManager vehiclesManager)
        {
            _vehiclesManager = vehiclesManager;
        }
        public async void OnGet()
        {
            if (HttpContext.Request.Query.ContainsKey("vehicleId"))
            {
                string vehicleId = HttpContext.Request.Query["vehicleId"];
                if (Int32.TryParse(vehicleId, out int i_vehicleId))
                {
                    int ownerId = await _vehiclesManager.GetVehicleOwnerId(i_vehicleId);
                    int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);

                    if (ownerId == userId)
                    {
                        ViewData["isCorrect"] = true;
                    }
                    else
                    {
                        ViewData["isCorrect"] = false;
                    }
                }
                else
                {
                    ViewData["isCorrect"] = false;
                }
            }
            else
            {
                ViewData["isCorrect"] = false;
            }
        }
    }
}
