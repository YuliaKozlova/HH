using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vacancyService.Data;
using vacancyService.Models;
using System.Text;
using vacancyService.Models.JsonBindings;
using EasyNetQ;
using System.Collections.Concurrent;
using RabbitModels;
using System.Threading;

namespace vacancyService.Controllers
{
    [Produces("application/json")]
    [Route("api/Emploer")]
    public class EmployerController : Controller
    {
        private readonly vacancyContext _context;

        public EmployerController(vacancyContext context)
        {
            _context = context;
        }

        // GET: api/Employer/Secret
        [Route("Secret")]
        [HttpGet]
        public IEnumerable<employer> GetEmployerSecret()
        {
            IEnumerable<employer> Employer = _context.Employer;

            var Bus = RabbitHutch.CreateBus("host=localhost");
            ConcurrentStack<Rabbitvacancyemployer> vacancyemployerCollection = new ConcurrentStack<Rabbitvacancyemployer>();

            Bus.Receive<Rabbitvacancyemployer>("vacancyemployer", msg =>
            {
                vacancyemployerCollection.Push(msg);
            });
            Thread.Sleep(5000);

            //foreach (Rabbitvacancyemployer a in vacancyemployerCollection)
            //{
            //    employer c = new employer() { employerName = a.employerName, EmployerAddress = a.EmployerAddress };
            //    _context.Employer.Add(c);
            //}
            //_context.SaveChanges();

            foreach (Rabbitvacancyemployer a in vacancyemployerCollection)
            {
                int c_id = 0;
                foreach (employer c in _context.Employer)
                {
                    if (a.employerName == c.employerName)
                        c_id = c.ID;
                }
                
                vacancy ar = new vacancy() { vacancyName = a.vacancyName, salary = a.vacancyPageCapacity, employerID = c_id};
                _context.vacancys.Add(ar);
            }
            _context.SaveChanges();

            return Employer;
        }

        // GET: api/Employer
        [HttpGet]
        public IEnumerable<employer> GetEmployer()
        {
            IEnumerable<employer> Employer = _context.Employer;

            //var Bus = RabbitHutch.CreateBus("host=localhost");
            //ConcurrentStack<Rabbitvacancyemployer> vacancyemployerCollection = new ConcurrentStack<Rabbitvacancyemployer>();

            //Bus.Receive<Rabbitvacancyemployer>("vacancyemployer", msg =>
            //{
            //    vacancyemployerCollection.Push(msg);
            //});
            //Thread.Sleep(5000);

            //foreach (Rabbitvacancyemployer a in vacancyemployerCollection)
            //{
            //    employer c = new employer() { employerName = a.employerName, AuthorRate = a.AuthorRate };
            //    _context.Employer.Add(c);
            //}
            //_context.SaveChanges();

            //foreach (Rabbitvacancyemployer a in vacancyemployerCollection)
            //{
            //    int c_id = 0;
            //    foreach (employer c in _context.Employer)
            //    {
            //        if (a.employerName == c.employerName)
            //            c_id = c.ID;
            //    }

            //    vacancy ar = new vacancy() { vacancyName = a.vacancyName, PageCapacity = a.vacancyPageCapacity, employerID = c_id };
            //    _context.vacancys.Add(ar);
            //}
            //_context.SaveChanges();

            return Employer;
        }

        // GET: api/Employer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Getemployer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employer = await _context.Employer.SingleOrDefaultAsync(m => m.ID == id);

            if (employer == null)
            {
                return NotFound();
            }

            return Ok(employer);
        }

        // PUT: api/Employer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putemployer([FromRoute] int id, [FromBody] employer employer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employer.ID)
            {
                return BadRequest();
            }

            _context.Entry(employer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Accepted(employer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!employerExists(id))
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



        // POST: api/Employer
        [HttpPost]
        public async Task<IActionResult> Postemployer([FromBody] employer employer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Employer.Add(employer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getemployer", new { id = employer.ID }, employer);
        }


        // DELETE: api/Employer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteemployer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employer = await _context.Employer.SingleOrDefaultAsync(m => m.ID == id);
            if (employer == null)
            {
                return NotFound();
            }

            _context.Employer.Remove(employer);
            await _context.SaveChangesAsync();

            return Ok(employer);
        }

        private bool employerExists(int id)
        {
            return _context.Employer.Any(e => e.ID == id);
        }


        // POST: api/Employer/Find
        [Route("Find")]
        [HttpPost]
        public async Task<IActionResult> FindByName([FromBody] employerBinding employerBinding)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employer = await _context.Employer.FirstOrDefaultAsync(m => m.employerName == employerBinding.Name);

            if (employer == null)
            {
                return NotFound();
            }

            return Ok(employer);
        }
    }
}