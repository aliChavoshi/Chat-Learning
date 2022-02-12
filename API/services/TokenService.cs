using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<Users> _userManager;
        public TokenService(IConfiguration config, UserManager<Users> userManager)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]));
            _userManager = userManager;
        }
        public async Task<string> CreateToken(Users user)
        {
            var claims = new List<Claim>{
                new(JwtRegisteredClaimNames.NameId, user.UserName),
                new(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            };
            //get role and add to claims
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}