using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refuel.Areas.Panel.Pages.Components.RefuelsForm;

namespace Refuel.Models
{
    public class RefuelFormModel
    {
        public RefuelsFormMode RefuelFormMode { get; set; }
        public InputRefuelModel Input { get; set; }
    }
}
