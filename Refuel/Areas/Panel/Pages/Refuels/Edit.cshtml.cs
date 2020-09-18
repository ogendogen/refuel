using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public List<string> ModelErrors { get; set; }

        private readonly IRefuelsManager _refuelsManager;
        public EditModel(IRefuelsManager refuelsManager)
        {
            _refuelsManager = refuelsManager;
            ModelErrors = new List<string>();
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
                Date = refuel.Date.ToString("yyyy-MM-dd HH:mm"),
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

                Vehicle refuelsVehicle = _refuelsManager.GetRefuelsVehicle(refuelId);

                if (DateTime.TryParseExact(Input.Date, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime refuelDateDt))
                {
                    _refuelsManager.Update(new RefuelType()
                    {
                        ID = refuelId,
                        Kilometers = Input.Kilometers,
                        Date = refuelDateDt,
                        Liters = Input.Liters,
                        PricePerLiter = Input.PricePerLiter,
                        Combustion = Input.Combustion,
                        Fuel = Input.Fuel,
                        TotalPrice = Input.TotalPrice,
                        Vehicle = refuelsVehicle
                    });
                }

                TempData["status"] = "edited";
                return RedirectToPage("List", new {vehicleId = refuelsVehicle.ID});
            }
            else
            {
                SaveAllErrors(ModelState.Values);
            }
            
            ErrorMessage = "Zweryfikuj poprawnoœæ formularza!";
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
