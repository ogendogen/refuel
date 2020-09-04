using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Refuel.Models;

namespace Refuel.Areas.Panel.Pages.Components.VehiclesForm
{
    public class VehiclesFormViewComponent : ViewComponent
    {
        public string ReadonlyValue { get; set; } = "false";
        public VehiclesFormViewComponent()
        {

        }

        public IViewComponentResult Invoke(VehiclesFormMode vehiclesFormMode, InputVehicleModel inputVehicleModel)
        {
            if (vehiclesFormMode == VehiclesFormMode.Forbidden)
            {
                return View("Forbidden");
            }

            if (vehiclesFormMode == VehiclesFormMode.Edit)
            {
                ReadonlyValue = "true";
            }

            VehicleFormModel vehicleFormModel = new VehicleFormModel()
            {
                VehiclesFormMode = vehiclesFormMode,
                Input = inputVehicleModel
            };

            return View("Form", vehicleFormModel);
        }
    }
}
