using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyService.Data;
using RabbitDLL;

namespace VacancyService.Controllers
{
    [Produces("application/json")]
    [Route("api/VacancyItems")]
    public class VacancyItemsController : Controller
    {
        private readonly VacancyContext _context;

        public VacancyItemsController(VacancyContext context)
        {
            _context = context;
        }

        // GET: api/VacancyItems
        [HttpGet]
        public IEnumerable<VacancyItems> GetVacancyItems()
        {
            return _context.VacancyItems;
        }

        // GET: api/VacancyItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVacancyItems([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var VacancyItems = await _context.VacancyItems.SingleOrDefaultAsync(m => m.ID == id);

            if (VacancyItems == null)
            {
                return NotFound();
            }

            return Ok(VacancyItems);
        }

        // PUT: api/VacancyItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVacancyItems([FromRoute] int id, [FromBody] VacancyItems VacancyItems)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != VacancyItems.ID)
            {
                return BadRequest();
            }

            _context.Entry(VacancyItems).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacancyItemsExists(id))
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

        // POST: api/VacancyItems
        [HttpPost]
        public async Task<IActionResult> PostVacancyItems([FromBody] VacancyItems VacancyItems)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.VacancyItems.Add(VacancyItems);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVacancyItems", new { id = VacancyItems.ID }, VacancyItems);
        }

        // DELETE: api/VacancyItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacancyItems([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var VacancyItems = await _context.VacancyItems.SingleOrDefaultAsync(m => m.ID == id);
            if (VacancyItems == null)
            {
                return NotFound();
            }

            _context.VacancyItems.Remove(VacancyItems);
            await _context.SaveChangesAsync();

            return Ok(VacancyItems);
        }

        private bool VacancyItemsExists(int id)
        {
            return _context.VacancyItems.Any(e => e.ID == id);
        }
    }
}