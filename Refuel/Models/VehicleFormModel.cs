using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Refuel.Areas.Panel.Pages.Components.VehiclesForm;

namespace Refuel.Models
{
    public class VehicleFormModel
    {
        public VehiclesFormMode VehiclesFormMode { get; set; }
        public InputVehicleModel Input { get; set; }
    }
}
