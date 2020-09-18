using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refuel.Models;
using Database.Models;
using RefuelType = Database.Models.Refuel;

namespace Refuel.Areas.Panel.Pages.Refuels
{
    public class DetailsModel : PageModel
    {
        [BindProperty]
        public InputRefuelModel Input { get; set; }
        private readonly IRefuelsManager _refuelsManager;
        public DetailsModel(IRefuelsManager refuelsManager)
        {
            _refuelsManager = refuelsManager;
        }

        public IActionResult OnGet(int refuelId)
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);

            if (!_refuelsManager.IsUserOwnsRefuel(userId, refuelId))
            {
                return Forbid();
            }

            RefuelType refuel = _refuelsManager.GetRefuelById(refuelId);
            Input = new InputRefuelModel()
            {
                Date = refuel.Date.ToString("dd-MM-yyyy HH:mm"),
                Kilometers = refuel.Kilometers,
                Liters = refuel.Liters,
                PricePerLiter = refuel.PricePerLiter,
                Combustion = refuel.Combustion,
                Fuel = refuel.Fuel,
                TotalPrice = refuel.TotalPrice
            };

            return Page();
        }

        public IActionResult OnPost(int refuelId)
        {
            int vehicleIdVar = _refuelsManager.GetRefuelsVehicle(refuelId).ID;
            return RedirectToPage("List", new {vehicleId = vehicleIdVar});
        }
    }
}
