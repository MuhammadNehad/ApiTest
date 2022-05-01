using ApiTest.Interfaces;
using ApiTest.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiTest.helpers
{
    public class JwtHelper : JwtInterface
    {
        private IConfiguration _config;
        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }
        public string generateJwt(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var handler = new JwtSecurityTokenHandler();

            var token = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(
                    claims),
                Expires= DateTime.UtcNow.AddHours(2).AddMinutes(400),
                    SigningCredentials= credentials
                };
            var tokenCreation = handler.CreateToken(token);
            return handler.WriteToken(tokenCreation);
        }
    }
}
