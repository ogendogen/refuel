using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models.NonDb
{
    public class DataPerFuelType
    {
        public Dictionary<FuelType, decimal> CostsPerFuelType { get; set; }
        public Dictionary<FuelType, decimal> AverageCombustionPerFuelType { get; set; }
        public Dictionary<FuelType, decimal> PriceFor100KmPerFuelType { get; set; }
        public DataPerFuelType()
        {
            CostsPerFuelType = new Dictionary<FuelType, decimal>();
            AverageCombustionPerFuelType = new Dictionary<FuelType, decimal>();
            PriceFor100KmPerFuelType = new Dictionary<FuelType, decimal>();
        }
    }
}
