using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServeNObserve.DataModels.Request
{
    public class LoginRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
