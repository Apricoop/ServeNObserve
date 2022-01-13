using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServeNObserve.Domain.Models
{
    public class Employee : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Salary { get; set; }
        public int CarId { get; set; } // ServiceId

        public bool IsCorrect()
        {
            return !(CarId < 0 || Salary < 0);
        }
    }
}
