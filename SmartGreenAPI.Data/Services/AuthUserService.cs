using Microsoft.Extensions.Configuration;
using SmartGreenAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace SmartGreenAPI.Data.Services
{
    public class AuthUserService
    {
        private readonly IConfiguration _configuration;

        public AuthUserService(IConfiguration configuration) => _configuration = configuration;

        public string GenerateJwtToken(UserModel user)
        {
            string secret = _configuration["JwtSettings:Secret"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Correo!),
                new Claim("id", user.Id!.ToString()),
                new Claim("role", user.UsuarioTipo!.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                //issuer: _configuration["JwtSettings:Issuer"],
                //audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
