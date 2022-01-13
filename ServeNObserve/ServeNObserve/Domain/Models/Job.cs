using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ServeNObserve.Shared;

namespace ServeNObserve.Domain.Models
{
    public class Job : BaseModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Water { get; set; }
        public int Gas { get; set; }
        public int Electricity { get; set; }
        public int Other { get; set; }
        public int Sgk { get; set; }
        public int Income { get; set; }
        public bool IsCompleted { get; set; } = false;


        [NotMapped] 
        public int TotalCost => Water + Gas + Electricity + Other + EmployeeSalaryCost + Sgk;
        [NotMapped] public int Balance => Income - TotalCost;
        [NotMapped] public int CarCost { get; set; }
        [NotMapped] public int EmployeeCount { get; set; }
        [NotMapped] public int EmployeeSalaryCost { get; set; }
        [NotMapped] public int DailySuitCost => Variables.SuitCost;
        [NotMapped] public int DailyFuelCost => Variables.DailyFuelCost;

        public bool IsCorrect()
        {
            return !(StartDate < DateTime.Now.AddDays(-1) || Electricity < 0 || Gas < 0 || Water < 0);
        }
    }
}
