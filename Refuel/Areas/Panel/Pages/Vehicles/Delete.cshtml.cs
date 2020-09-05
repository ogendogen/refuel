using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Refuel.Areas.Panel.Pages.Vehicles
{
    public class DeleteModel : PageModel
    {
        private readonly IVehiclesManager _vehiclesManager;

        public string VehicleName { get; set; }
        
        public DeleteModel(IVehiclesManager vehiclesManager)
        {
            _vehiclesManager = vehiclesManager;
        }

        public void OnGet(int vehicleId)
        {
            VehicleName = _vehiclesManager.GetVehicleManufacturerAndModelById(vehicleId);
        }

        public async Task<IActionResult> OnPostAsync(int vehicleId)
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);
            Vehicle vehicle = _vehiclesManager.GetVehicleById(vehicleId);

            if (vehicleId == vehicle.ID)
            {
                await _vehiclesManager.Delete(vehicle);
                TempData["status"] = "deleted";
                return RedirectToPage("Index");
            }

            TempData["status"] = "forbidden";
            return RedirectToPage("Index");
        }
    }
}
