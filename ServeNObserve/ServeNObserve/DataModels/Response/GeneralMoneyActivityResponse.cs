using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServeNObserve.DataModels.Response
{
    public class GeneralMoneyActivityResponse
    {
        public DateTime Date { get; set; }
        public int Outcome { get; set; }
        public int Income { get; set; }
    }
}
