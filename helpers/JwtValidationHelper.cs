using ApiTest.Data;
using ApiTest.Interfaces;
using ApiTest.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTest.helpers
{
    public class JwtValidationHelper : JwtValidationInterface
    {
        private IConfiguration _config;
        private readonly ApiTestContext _context;

        public JwtValidationHelper(IConfiguration config,ApiTestContext context)
        {
            _config = config;
            _context = context;

        }
        public bool validate(string user, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            tokenHandler.ValidateToken(
                token, new TokenValidationParameters {
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))

                },out SecurityToken validatedToken);
            var jwttoken = (JwtSecurityToken)validatedToken;
            var username =jwttoken.Claims.First(c => c.Type == "sub").Value;
            var suser = _context.UserModel.Where(u=>u.name ==username).FirstOrDefault();
            if(suser != null && username == user)
            {
                return true;
            }
            return false;

        }
    }
}
