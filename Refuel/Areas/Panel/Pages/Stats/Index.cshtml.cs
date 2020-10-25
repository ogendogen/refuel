using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refuel.Models;

namespace Refuel.Areas.Panel.Pages.Stats
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public VehiclesListModel VehicleListModel { get; set; }
        public List<Vehicle> UserVehicles { get; set; }
        private readonly IVehiclesManager _vehiclesManager;

        public IndexModel(IVehiclesManager vehiclesManager)
        {
            _vehiclesManager = vehiclesManager;
            UserVehicles = new List<Vehicle>();
        }
        public void OnGet()
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);
            UserVehicles = _vehiclesManager.GetAllUserVehicles(userId).ToList();
        }

        public IActionResult OnPost()
        {
            if (Int32.TryParse(Request.Form["VehicleListModel"][0], out int vehicleId))
            {
                return RedirectToPage("VehicleStats", new { vehicleId = vehicleId });
            }

            return Page();
        }
    }
}
