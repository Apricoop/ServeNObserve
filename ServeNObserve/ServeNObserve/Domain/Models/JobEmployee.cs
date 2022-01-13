using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServeNObserve.Domain.Models
{
    public class JobEmployee : BaseModel
    {
        public int JobId { get; set; }
        public int EmpoloyeeId { get; set; }
    }
}
