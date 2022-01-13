using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServeNObserve.Domain.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        // Giden response icerisinde gorunmesi isteniyorsa JsonIgnore kaldirilmali
        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public bool isActive { get; set; } = true;
    }
}
