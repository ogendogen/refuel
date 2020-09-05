using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refuel.Areas.Panel.Pages.Components.VehiclesForm;
using Refuel.Models;

namespace Refuel.Areas.Panel.Pages.Vehicles
{
    public class EditModel : PageModel
    {
        private readonly IVehiclesManager _vehiclesManager;
        private readonly IUsersManager _usersManager;

        [BindProperty]
        public InputVehicleModel Input { get; set; }
        public string ErrorMessage { get; set; }
        public EditModel(IVehiclesManager vehiclesManager, IUsersManager usersManager)
        {
            _vehiclesManager = vehiclesManager;
            _usersManager = usersManager;
        }

        public IActionResult OnGet(string vehicleId)
        {
            ErrorMessage = "";
            if (Int32.TryParse(vehicleId, out int i_vehicleId))
            {
                int? ownerId = _vehiclesManager.GetVehicleOwnerId(i_vehicleId).Result;
                if (ownerId == null)
                {
                    TempData["status"] = "forbidden";
                    return RedirectToPage("Index");
                }

                int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);

                if (ownerId == userId)
                {
                    Vehicle vehicle = _vehiclesManager.GetVehicleById(i_vehicleId);
                    Input = new InputVehicleModel()
                    {
                        Manufacturer = vehicle.Manufacturer,
                        Model = vehicle.Model,
                        Engine = vehicle.Engine,
                        Horsepower = vehicle.Horsepower,
                        Description = vehicle.Description
                    };
                }
                else
                {
                    TempData["status"] = "forbidden";
                    return RedirectToPage("Index");
                }

                return Page();
            }

            TempData["status"] = "forbidden";
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPost(string vehicleId)
        {
            if (ModelState.IsValid && Int32.TryParse(vehicleId, out int i_vehicleId))
            {
                int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);
                User owner = await _usersManager.GetUserById(userId);

                if (owner.ID == userId)
                {
                    await _vehiclesManager.Update(new Vehicle()
                    {
                        ID = i_vehicleId,
                        Manufacturer = Input.Manufacturer,
                        Model = Input.Model,
                        Engine = Input.Engine,
                        Horsepower = Input.Horsepower,
                        Description = Input.Description,
                        Owner = owner
                    });
                }
                else
                {
                    ErrorMessage = "Dany pojazd nie istnieje lub nie jesteœ jego posiadaczem!";
                    return Page();
                }

                TempData["status"] = "edited";
                return RedirectToPage("Index");
            }

            ErrorMessage = "Zweryfikuj poprawnoœæ formularza!";
            return Page();
        }
    }
}
