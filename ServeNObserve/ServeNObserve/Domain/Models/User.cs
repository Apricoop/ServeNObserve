using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ServeNObserve.DataModels.Request;

namespace ServeNObserve.Domain.Models
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        [JsonIgnore]
        public string HashedPassword { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }

        public User() { }

        public User(CreateUserRequest request, Tuple<string, string> hashTuple)
        {
            Email = request.Email;
            CreatedDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            isActive = true;
            Salt = hashTuple.Item1;
            HashedPassword = hashTuple.Item2;
            FirstName = request.FirstName;
            LastName = request.LastName;
        }
    }
}
