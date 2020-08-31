using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Refuel.Models;

namespace Refuel.Areas.Panel.Pages.Vehicles
{
    public class AddModel : PageModel
    {
        private readonly IVehiclesManager _vehiclesManager;
        private readonly IUsersManager _usersManager;

        [BindProperty]
        public InputVehicleModel Input { get; set; }
        public AddModel(IVehiclesManager vehiclesManager, IUsersManager usersManager)
        {
            _vehiclesManager = vehiclesManager;
            _usersManager = usersManager;
        }
        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                int userId = Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value);
                User owner = await _usersManager.GetUserById(userId);
                await _vehiclesManager.Add(Input.Manufacturer, Input.Model, Input.Engine, Input.HorsePower, Input.Description, owner);

                ViewData["isSuccess"] = true;
            }
            else
            {
                ViewData["isSuccess"] = false;
            }

            // Todo: redirect to index and show alert for success
        }
    }
}
