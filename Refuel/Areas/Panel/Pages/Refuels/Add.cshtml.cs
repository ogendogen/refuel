using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refuel.Models;

namespace Refuel.Areas.Panel.Pages.Refuels
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public InputRefuelModel Input { get; set; }

        private readonly IRefuelsManager _refuelsManager;
        private readonly IVehiclesManager _vehiclesManager;

        public AddModel(IRefuelsManager refuelsManager, IVehiclesManager vehiclesManager)
        {
            _refuelsManager = refuelsManager;
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

        public async Task<IActionResult> OnPost(int vehicleId)
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);
            if (!_vehiclesManager.IsUserOwnsVehicle(userId, vehicleId))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var refuel = await _refuelsManager.Add(date: Input.Date,
                    kilometers: Input.Kilometers,
                    pricePerLiter: Input.PricePerLiter,
                    liters: Input.Liters,
                    combustion: Input.Combustion,
                    fuel: Input.Fuel,
                    totalPrice: Input.TotalPrice);

                TempData["status"] = "added";
                return RedirectToPage("List", new {vehicleId = vehicleId});
            }

            return Page();
        }
    }
}
