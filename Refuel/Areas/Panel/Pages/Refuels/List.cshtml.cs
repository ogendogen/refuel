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

        private readonly IVehiclesManager _vehiclesManager;

        public ListModel(IVehiclesManager vehiclesManager)
        {
            _vehiclesManager = vehiclesManager;
        }
        public void OnGet(int vehicleId)
        {
            Vehicle vehicle = _vehiclesManager.GetVehicleById(vehicleId);
            Refuels = _vehiclesManager.GetAllVehicleRefuelsSortedDescendingByDate(vehicle).ToList();
        }
    }
}
