using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTest.Data;
using ApiTest.Models;
using Microsoft.AspNetCore.Authorization;
using ApiTest.Filters;

namespace ApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TokenFilter]
    public class UserBeersController : ControllerBase
    {
        private readonly ApiTestContext _context;

        public UserBeersController(ApiTestContext context)
        {
            _context = context;
        }

        // GET: api/UserBeers

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBeer>>> GetUserBeer()
        {
            return await _context.UserBeer.ToListAsync();
        }

        // GET: api/UserBeers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserBeer>> GetUserBeer(int id)
        {
            var userBeer = await _context.UserBeer.FindAsync(id);

            if (userBeer == null)
            {
                return NotFound();
            }

            return userBeer;
        }

        // PUT: api/UserBeers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserBeer(int id, UserBeer userBeer)
        {
            if (id != userBeer.BeerDataId)
            {
                return BadRequest();
            }

            _context.Entry(userBeer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserBeerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserBeers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserBeer>> PostUserBeer(UserBeer userBeer)
        {
           var  _userBeer =_context.UserBeer.Find(userBeer.BeerDataId,userBeer.UserModelId );
            if(_userBeer != null)
            {
                _context.UserBeer.Remove(_userBeer);
                _context.SaveChanges();
            }

            if(userBeer.BeerDataId== 0 || userBeer.UserModelId == 0 || userBeer.rate ==null)
            {
                return BadRequest();
            }

            _context.UserBeer.Add(userBeer);
            try
            {
                await _context.SaveChangesAsync();

                var _userBeers = _context.UserBeer.Where(ub => ub.BeerDataId == userBeer.BeerDataId).ToList();
                
                var sum = _userBeers.GroupBy(item=> item.BeerDataId).Select(ubg=>ubg.Sum(item=> item.rate)).ToList();


                var _Beer = _context.BeerData.Find(userBeer.BeerDataId);
                _Beer.rating = sum[0] / _userBeers.Count;

                _context.BeerData.Update(_Beer);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserBeerExists(userBeer.BeerDataId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserBeer", new { id = userBeer.BeerDataId }, userBeer);
        }

        // DELETE: api/UserBeers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserBeer(int id)
        {
            var userBeer = await _context.UserBeer.FindAsync(id);
            if (userBeer == null)
            {
                return NotFound();
            }

            _context.UserBeer.Remove(userBeer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserBeerExists(int id)
        {
            return _context.UserBeer.Any(e => e.BeerDataId == id);
        }
    }
}
