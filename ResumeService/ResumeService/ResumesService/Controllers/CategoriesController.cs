using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumesService.Data;
using RabbitDLL;

namespace ResumesService.Controllers
{
    [Produces("application/json")]
    [Route("api/Categories")]
    public class CategoriesController : Controller
    {
        private readonly ResumeContext _context;

        public CategoriesController(ResumeContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public IEnumerable<Company> GetCategories()
        {
            return _context.Categories;
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Company = await _context.Categories.SingleOrDefaultAsync(m => m.ID == id);

            if (Company == null)
            {
                return NotFound();
            }

            return Ok(Company);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany([FromRoute] int id, [FromBody] Company Company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Company.ID)
            {
                return BadRequest();
            }

            _context.Entry(Company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> PostCompany([FromBody] Company Company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Categories.Add(Company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = Company.ID }, Company);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Company = await _context.Categories.SingleOrDefaultAsync(m => m.ID == id);
            if (Company == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(Company);
            await _context.SaveChangesAsync();

            return Ok(Company);
        }

        private bool CompanyExists(int id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }
    }
}