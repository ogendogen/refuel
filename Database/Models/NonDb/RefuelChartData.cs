using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models.NonDb
{
    public class RefuelChartData
    {
        public DateTime RefuelDate { get; set; }
        public decimal Price { get; set; }
        public decimal Combustion { get; set; }
    }
}
