using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utils;

namespace Refuel.Areas.Panel.Pages.Components.RefuelsForm
{
    public static class EnumListExtensions
    {
        public static IEnumerable<SelectListItem> GetDescribedFuelTypes(this IEnumerable<SelectListItem> rawFuelTypes)
        {
            foreach (var fuelType in rawFuelTypes)
            {
                if (Enum.TryParse(fuelType.Text, out FuelType fuelTypeEnum))
                {
                    fuelType.Text = fuelTypeEnum.Description();
                    yield return fuelType;
                }
            }
        }
    }
}
