using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServeNObserve.DataModels.Response
{
    public class JobCostResponse
    {
        public DateTime Date { get; set; }
        public int GeneralCost { get; set; }
        public int DailyFuelCost { get; set; }
        public int TransportationCost { get; set; }
        public int SalaryCost { get; set; }
        public int Total => GeneralCost + DailyFuelCost + TransportationCost + SalaryCost;
    }
}
