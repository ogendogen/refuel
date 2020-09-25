using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refuel.APIModels
{
    public class RefuelStatsCorrectResponse : RefuelStatsAPIResponse
    {
        public decimal AverageCombustion { get; set; }
        public decimal PriceFor100Km { get; set; }
    }
}
