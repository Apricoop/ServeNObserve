using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServeNObserve.Domain.Models;

namespace ServeNObserve.Shared
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}
