using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models.NonDb
{
    public class VehicleCosts
    {
        public decimal TotalCosts { get; set; }
        public Dictionary<FuelType, decimal> CostsPerFuelType { get; set; }
        public VehicleCosts()
        {
            CostsPerFuelType = new Dictionary<FuelType, decimal>();
        }
    }
}
