using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refuel.Models;

namespace Refuel.Areas.Panel.Pages.Refuels
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public InputRefuelModel Input { get; set; }
        public List<string> ModelErrors { get; set; }

        private readonly IRefuelsManager _refuelsManager;
        private readonly IVehiclesManager _vehiclesManager;

        public AddModel(IRefuelsManager refuelsManager, IVehiclesManager vehiclesManager)
        {
            _refuelsManager = refuelsManager;
            _vehiclesManager = vehiclesManager;
            ModelErrors = new List<string>();
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

        public async Task<IActionResult> OnPost(int vehicleId)
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);
            if (!_vehiclesManager.IsUserOwnsVehicle(userId, vehicleId))
            {
                return Forbid();
            }

            Vehicle vehicle = _vehiclesManager.GetVehicleById(vehicleId);

            if (ModelState.IsValid)
            {
                if (DateTime.TryParseExact(Input.Date, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime refuelDateDt))
                {
                    var refuel = await _refuelsManager.Add(date: refuelDateDt,
                        kilometers: Input.Kilometers,
                        pricePerLiter: Input.PricePerLiter,
                        liters: Input.Liters,
                        combustion: Input.Combustion,
                        fuel: Input.Fuel,
                        totalPrice: Input.TotalPrice,
                        vehicle: vehicle);
                }

                TempData["status"] = "added";
                return RedirectToPage("List", new {vehicleId = vehicleId});
            }
            else
            {
                SaveAllErrors(ModelState.Values);
            }

            return Page();
        }

        private void SaveAllErrors(ModelStateDictionary.ValueEnumerable values)
        {
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    ModelErrors.Add(error.ErrorMessage);
                }
            }
        }
    }
}
