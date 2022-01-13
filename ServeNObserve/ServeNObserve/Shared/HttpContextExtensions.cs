using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ServeNObserve.Domain.Models;
using ServeNObserve.Persistence.Contexts;

namespace ServeNObserve.Shared
{
    public static class HttpContextExtensions
    {
        public static User GetThisUser(this HttpContext context, DatabaseContext _dbContext)
        {
            if (context.User != null && context.User.Claims.Count() > 0)
            {
                var claims = context.User.Claims.ToDictionary(u => u.Type, u => u.Value);
                string userId = claims[ClaimTypes.NameIdentifier].ToString();

                return _dbContext.Users.Where(s => s.Id == int.Parse(userId) && s.isActive).FirstOrDefault();
            }
            return null;
        }
    }
}
