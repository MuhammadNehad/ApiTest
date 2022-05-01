using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTest.Data;
using ApiTest.Models;
using ApiTest.helpers;
using ApiTest.Interfaces;

namespace ApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        private readonly ApiTestContext _context;
        private readonly JwtInterface _jwtHelper;

        public Authentication(ApiTestContext context, JwtInterface jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }


        // POST: api/Authentication
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public  ActionResult<UserModel> PostUserModel(UserModel userModel)
        {

            var user = Authenticate(userModel);
            if(user!=null)
            {
                var tokenString = _jwtHelper.generateJwt(user);
                return Ok(new { token = tokenString });
            }
            return Unauthorized();
        }

        private UserModel Authenticate(UserModel userModel)
        {
            UserModel user = null;
            if (userModel != null)
            {
                user = _context.UserModel.Where(u => u.name== userModel.name &&  u.password == userModel.password).FirstOrDefault();
            }
            
            return user;
        }


    }
}
