using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models.NonDb;

namespace Refuel.APIModels
{
    public class RefuelStatsVehicleFuelStats : RefuelStatsAPIResponse
    {
        public List<RefuelChartData> RefuelsDataForCharts { get; set; }
        public decimal FuelTotalPrice { get; set; }
        public RefuelStatsVehicleFuelStats()
        {
            RefuelsDataForCharts = new List<RefuelChartData>();
        }
    }
}
