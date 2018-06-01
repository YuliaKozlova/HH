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
    [Route("api/Resume_Company")]
    public class Resume_CompanyController : Controller
    {
        //public class FullModel
        //{
        //    public string Speiality;
        //    public string ResumeName;      
        //    public string CompanyName;
        //    public string 
        //}

        private readonly ResumeContext _context;

        public Resume_CompanyController(ResumeContext context)
        {
            _context = context;
        }

        // GET: api/Resume_Company
        [HttpGet]
        public IEnumerable<Resume_Company> GetResume_Categories()
        {
            return _context.Resume_Categories;
        }

        // GET: api/Resume_Company/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResume_Company([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Resume_Company = await _context.Resume_Categories.SingleOrDefaultAsync(m => m.ID == id);

            if (Resume_Company == null)
            {
                return NotFound();
            }

            return Ok(Resume_Company);
        }

        // PUT: api/Resume_Company/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResume_Company([FromRoute] int id, [FromBody] Resume_Company Resume_Company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Resume_Company.ID)
            {
                return BadRequest();
            }

            _context.Entry(Resume_Company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Resume_CompanyExists(id))
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

        // POST: api/Resume_Company
        [HttpPost]
        public async Task<IActionResult> PostResume_Company([FromBody] Resume_Company Resume_Company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Resume_Categories.Add(Resume_Company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResume_Company", new { id = Resume_Company.ID }, Resume_Company);
        }

        // DELETE: api/Resume_Company/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResume_Company([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Resume_Company = await _context.Resume_Categories.SingleOrDefaultAsync(m => m.ID == id);
            if (Resume_Company == null)
            {
                return NotFound();
            }

            _context.Resume_Categories.Remove(Resume_Company);
            await _context.SaveChangesAsync();

            return Ok(Resume_Company);
        }

        private bool Resume_CompanyExists(int id)
        {
            return _context.Resume_Categories.Any(e => e.ID == id);
        }
    }
}