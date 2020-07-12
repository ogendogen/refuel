using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Refuel
    {
        public int ID { get; set; }
        public int VehicleID { get; set; }
        public DateTime Date { get; set; }
        public uint Kilometers { get; set; }
        public decimal PricePerLiter { get; set; }
        public decimal Liters { get; set; }
        public decimal Combustion { get; set; }
        public FuelType Fuel { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
