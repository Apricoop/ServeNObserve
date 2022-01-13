using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServeNObserve.Domain.Models
{
    public class ServiceEmployee : BaseModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int EmpoloyeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}
