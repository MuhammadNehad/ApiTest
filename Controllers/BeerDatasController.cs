using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class BeerDatasController : ControllerBase
    {
        private readonly ApiTestContext _context;

        public BeerDatasController(ApiTestContext context)
        {
            _context = context;
        }

        // GET: api/BeerDatas
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerData>>> GetBeerData()
        {
            return await _context.BeerData.ToListAsync();
        }

        // GET: api/BeerDatas?name=
        [AllowAnonymous]
        [HttpGet("GetByName")]
        public async Task<ActionResult<IEnumerable<BeerData>>> GetBeerData([FromQuery]string name)
        {
            return await _context.BeerData.Where(b=>b.name.Contains(name)).ToListAsync();
        }

        // PUT: api/BeerDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkId=2123754
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutBeerData(int Id, BeerData beerData)
        {
            if (Id != beerData.Id)
            {
                return BadRequest();
            }
            var model = _context.Entry(beerData);

            model.State = EntityState.Modified;
            model.Property(e => e.Id).IsModified = false;
            if(beerData.name.checkIfNull())
            {
                model.Property(e => e.name).IsModified = false;
            }

            if(beerData.type== null && beerData.type.checkIfDefined())
            {
                model.Property(e => e.type).IsModified = false;
            }

            if (beerData.rating==null)
            {
                model.Property(e => e.rating).IsModified = false;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeerDataExists(Id))
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

        // POST: api/BeerDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkId=2123754
        [HttpPost]
        public async Task<ActionResult<BeerData>> PostBeerData([FromBody]BeerData beerData)
        {
            if(beerData.name.checkIfNull()|| beerData.type.checkIfDefined())
            {
                return BadRequest();
            }

            _context.BeerData.Add(beerData);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BeerDataExists(beerData.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBeerData", new { Id = beerData.Id }, beerData);
        }

        // DELETE: api/BeerDatas/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteBeerData(string Id)
        {
            var beerData = await _context.BeerData.FindAsync(Id);
            if (beerData == null)
            {
                return NotFound();
            }

            _context.BeerData.Remove(beerData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BeerDataExists(int Id)
        {
            return _context.BeerData.Any(e => e.Id == Id);
        }
    }
}
