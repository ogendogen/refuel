using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Refuel.Models;

namespace Refuel.Areas.Panel.Pages.Components.RefuelsForm
{
    public class RefuelsFormViewComponent : ViewComponent
    {
        public RefuelsFormViewComponent()
        {

        }
        public IViewComponentResult Invoke(RefuelsFormMode refuelsMode, InputRefuelModel inputRefuelModel)
        {
            RefuelFormModel refuelFormModel = new RefuelFormModel()
            {
                RefuelFormMode = refuelsMode,
                Input = inputRefuelModel
            };

            return View("Default", refuelFormModel);
        }
    }
}
