using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refuel.Models;
using Utils;

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
