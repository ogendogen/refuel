using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refuel.Models;
using RefuelType = Database.Models.Refuel;

namespace Refuel.Areas.Panel.Pages.Refuels
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public InputRefuelModel Input { get; set; }
        public string ErrorMessage { get; set; }
        private readonly IRefuelsManager _refuelsManager;
        public EditModel(IRefuelsManager refuelsManager)
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
                Date = refuel.Date,
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
            if (ModelState.IsValid)
            {
                int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);

                if (!_refuelsManager.IsUserOwnsRefuel(userId, refuelId))
                {
                    return Forbid();
                }

                _refuelsManager.Update(new RefuelType()
                {
                    ID = refuelId,
                    Kilometers = Input.Kilometers,
                    Date = Input.Date,
                    Liters = Input.Liters,
                    PricePerLiter = Input.PricePerLiter,
                    Combustion = Input.Combustion,
                    Fuel = Input.Fuel,
                    TotalPrice = Input.TotalPrice
                });

                Vehicle refuelsVehicle = _refuelsManager.GetRefuelsVehicle(refuelId);

                TempData["status"] = "edited";
                return RedirectToPage("List", refuelsVehicle.ID);
            }
            
            ErrorMessage = "Zweryfikuj poprawnoœæ formularza!";
            return Page();
        }
    }
}
