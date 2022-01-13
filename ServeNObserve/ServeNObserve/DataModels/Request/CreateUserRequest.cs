using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServeNObserve.DataModels.Request
{
    public class CreateUserRequest
    {
        [DataType(DataType.EmailAddress)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail is not valid.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        [StringLength(30, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Firstname is required.")]
        [StringLength(30, MinimumLength = 3)]
        public string FirstName { get; set; }
        
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }
    }
}
