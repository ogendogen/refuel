using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Database.Models;
using RefuelType = Database.Models.Refuel;
using Database;

namespace Refuel.Areas.Panel.Pages.Refuels
{
    public class ListModel : PageModel
    {
        [BindProperty]
        public List<RefuelType> Refuels { get; set; }
        public int VehicleID { get; set; }

        private readonly IVehiclesManager _vehiclesManager;

        public ListModel(IVehiclesManager vehiclesManager)
        {
            _vehiclesManager = vehiclesManager;
        }
        public IActionResult OnGet(int vehicleId)
        {
            VehicleID = vehicleId;
            int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);
            Vehicle vehicle = _vehiclesManager.GetVehicleById(vehicleId);
            if (userId != vehicle.Owner.ID)
            {
                return Forbid();
            }
            
            Refuels = _vehiclesManager.GetAllVehicleRefuelsSortedDescendingByDate(vehicle).ToList();
            return Page();
        }
    }
}
