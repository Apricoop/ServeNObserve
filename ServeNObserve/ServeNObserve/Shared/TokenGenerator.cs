using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServeNObserve.Domain.Models;

namespace ServeNObserve.Shared
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly AppSettings _appSettings;

        public TokenGenerator(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    //new Claim(ClaimTypes.Name, user.FirstName),
                    //new Claim(ClaimTypes.LastName, user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddDays(4), // You can change the tokens' expire time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha384Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
