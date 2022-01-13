using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace ServeNObserve.Domain.Models
{
    public class Service : BaseModel
    {
        public string Name { get; set; }
    }
}
