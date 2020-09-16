using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RefuelType = Database.Models.Refuel;

namespace Refuel.Areas.Panel.Pages.Refuels
{
    public class DeleteModel : PageModel
    {
        private readonly IRefuelsManager _refuelsManager;

        public int RefuelID { get; set; }

        public DeleteModel(IRefuelsManager refuelsManager)
        {
            _refuelsManager = refuelsManager;
        }

        public void OnGet(int refuelId)
        {
            RefuelID = refuelId;
        }

        public async Task<IActionResult> OnPostAsync(int refuelId)
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);
            if (!_refuelsManager.IsUserOwnsRefuel(userId, refuelId))
            {
                return Forbid();
            }

            RefuelType refuel = _refuelsManager.GetRefuelById(refuelId);
            int vehicleIdVar = refuel.Vehicle.ID;

            await _refuelsManager.Delete(refuel);
            TempData["status"] = "deleted";
            return RedirectToPage("List", new {vehicleId = vehicleIdVar});
        }
    }
}
