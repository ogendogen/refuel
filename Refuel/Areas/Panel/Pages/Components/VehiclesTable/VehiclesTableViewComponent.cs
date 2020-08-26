using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;

namespace Refuel.Areas.Panel.Pages.Components.VehiclesTable
{
    public class VehiclesTableViewComponent : ViewComponent
    {
        private readonly IUsersManager _usersManager;

        public VehiclesTableViewComponent(IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        public IViewComponentResult Invoke(int userId)
        {
            var userVehicles = _usersManager.GetUserAllVehicles(userId);
            return View("Default", userVehicles);
        }
    }
}
