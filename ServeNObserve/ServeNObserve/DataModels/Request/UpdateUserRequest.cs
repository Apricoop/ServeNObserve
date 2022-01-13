using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServeNObserve.DataModels.Request
{
    public class UpdateUserRequest
    {
        [StringLength(30, MinimumLength = 3)]
        public string Email { get; set; }
        [StringLength(30, MinimumLength = 3)]
        public string FirstName { get; set; }
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }
        [StringLength(30, MinimumLength = 6)]
        public string Password { get; set; }
        //[StringLength(14, MinimumLength = 10, ErrorMessage = "Maximum 14 and minimum 10 characters")]
        //public string GsmNumber { get; set; }
        //public IFormFile Image { get; set; }
        //public string City { get; set; }
        //public DateTime? DateofBirth { get; set; }
        //public EGender? Gender { get; set; } = EGender.unspecified;
    }
}
