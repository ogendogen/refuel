using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refuel.Models;

namespace Refuel.Areas.Panel.Pages.Vehicles
{
    public class DetailsModel : PageModel
    {
        [BindProperty]
        public InputVehicleModel Input { get; set; }
        private readonly IVehiclesManager _vehiclesManager;

        public DetailsModel(IVehiclesManager vehiclesManager)
        {
            _vehiclesManager = vehiclesManager;
        }
        
        public IActionResult OnGet(string vehicleId)
        {
            if (Int32.TryParse(vehicleId, out int i_vehicleId))
            {
                int? ownerId = _vehiclesManager.GetVehicleOwnerId(i_vehicleId).Result;
                if (ownerId == null)
                {
                    return RedirectToPage("Index");
                }

                int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);

                if (ownerId == userId)
                {
                    Vehicle vehicle = _vehiclesManager.GetVehicleById(i_vehicleId);
                    Input = new InputVehicleModel()
                    {
                        Model = vehicle.Model,
                        Manufacturer = vehicle.Manufacturer,
                        Engine = vehicle.Engine,
                        Horsepower = vehicle.Horsepower,
                        Description = vehicle.Description
                    };

                    return Page();
                }
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPost() // just in case
        {
            return Page();
        }
    }
}
