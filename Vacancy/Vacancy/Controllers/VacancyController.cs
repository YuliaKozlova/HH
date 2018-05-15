using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vacancyService.Data;
using vacancyService.Models;
using ReflectionIT.Mvc.Paging;
using vacancyService.Models.JsonBindings;

namespace vacancyService.Controllers
{
    [Produces("application/json")]
    [Route("api/vacancys")]
    public class vacancysController : Controller
    {
        const int StringsPerPage = 10;


        private readonly vacancyContext _context;


        public vacancysController(vacancyContext context)
        {
            _context = context;
        }


        // GET: api/vacancys/page/{id}
        [HttpGet]
        [Route("page/{page}")]
        public List<vacancy> Getvacancys([FromRoute] int page = 1)
        {
            var qry = _context.vacancys.OrderBy(p => p.vacancyName);
            foreach (vacancy a in qry)
            {
                _context.Entry(a).Navigation("employer").Load();
            }

            PagingList<vacancy> vacancysList;
            if (page != 0)
            {
                
                vacancysList = PagingList.Create(qry, StringsPerPage, page);
            }
            else
            {
                vacancysList = PagingList.Create(qry, _context.vacancys.Count() + 1, 1);
            }

            return vacancysList.ToList();
        }


        [HttpGet]
        public List<vacancy> GetvacancysAll()
        {
            var qry = _context.vacancys.OrderBy(p => p.vacancyName);
            foreach (vacancy a in qry)
            {
                _context.Entry(a).Navigation("employer").Load();
            }

            PagingList<vacancy> vacancysList;

            vacancysList = PagingList.Create(qry, _context.vacancys.Count() + 1, 1);

            return vacancysList.ToList();
        }


        //[HttpGet]
        //public IEnumerable<vacancy> GetvacancysAll()
        //{
        //    IEnumerable<vacancy> vacancys = _context.vacancys;
        //    foreach (vacancy a in vacancys)
        //    {
        //        _context.Entry(a).Navigation("employer").Load();
        //    }

        //    return vacancys;
        //}


        // GET: api/vacancys/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Getvacancy([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vacancy = await _context.vacancys.SingleOrDefaultAsync(m => m.ID == id);

            if (vacancy == null)
            {
                return NotFound();
            }

            return Ok(vacancy);
        }


        // PUT: api/vacancys/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putvacancy([FromRoute] int id, [FromBody] vacancy vacancy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vacancy.ID)
            {
                return BadRequest();
            }

            _context.Entry(vacancy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Accepted(vacancy);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vacancyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
        }


        // POST: api/vacancys
        [HttpPost]
        public async Task<IActionResult> Postvacancy([FromBody] vacancy vacancy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.vacancys.Add(vacancy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getvacancy", new { id = vacancy.ID }, vacancy);
        }


        // DELETE: api/vacancys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletevacancy([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vacancy = await _context.vacancys.SingleOrDefaultAsync(m => m.ID == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            _context.vacancys.Remove(vacancy);
            await _context.SaveChangesAsync();

            return Ok(vacancy);
        }

        private bool vacancyExists(int id)
        {
            return _context.vacancys.Any(e => e.ID == id);
        }

        // POST: api/vacancys/Find
        [Route("Find")]
        [HttpPost]
        public async Task<IActionResult> FindByName([FromBody] vacancyBinding vacancyBinding)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vacancy = await _context.vacancys.FirstOrDefaultAsync(m => m.vacancyName == vacancyBinding.Name);

            _context.Entry(vacancy).Navigation("employer").Load();

            if (vacancy == null)
            {
                return NotFound();
            }

            return Ok(vacancy);
        }


        // GET: api/vacancys
        [HttpGet]
        [Route("count")]
        public int GetCountvacancys()
        {
            return _context.vacancys.Count();
        }
    }
}