using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;

namespace Refuel.Models
{
    public class InputRefuelModel
    {
        public DateTime Date { get; set; }
        public uint Kilometers { get; set; }
        public decimal PricePerLiter { get; set; }
        public decimal Liters { get; set; }
        public decimal Combustion { get; set; }
        public FuelType Fuel { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
