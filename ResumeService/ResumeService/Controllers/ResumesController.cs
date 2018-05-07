using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcerteService.Data;
using ConcerteService.Models;
using ReflectionIT.Mvc.Paging;
using Resumeservice.Models.JsonBindings;
using EasyNetQ;
using RabbitModels;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace ConcerteService.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Resumes")]
    public class ResumesController : Controller
    {
        private readonly ResumeContext _context;

        const int StringsPerPage = 10;

        public ResumesController(ResumeContext context)
        {
            _context = context;
        }

        // GET: api/Resumes
        [HttpGet]
        public IEnumerable<Resume> GetResumes()
        {
            //var Bus = RabbitHutch.CreateBus("host=localhost");
            //ConcurrentStack<Resume> ResumesCollection = new ConcurrentStack<Resume>();

            //Bus.Receive<RabbitResume>("Resume", msg =>
            //{
            //    Resume Resume = new Resume() { ResumeName = msg.ResumeName, LastFmRating = msg.LastFmRating };
            //    ResumesCollection.Push(Resume);
            //});
            //Thread.Sleep(5000);

            //foreach (Resume a in ResumesCollection)
            //{
            //    _context.Add(a);
            //}
            //_context.SaveChanges();
            return _context.Resumes;
        }


        // GET: api/Resumes/Secret
        [Route("Secret")]
        [HttpGet]
        public IEnumerable<Resume> GetResumesSecret()
        {
            var Bus = RabbitHutch.CreateBus("host=localhost");
            ConcurrentStack<Resume> ResumesCollection = new ConcurrentStack<Resume>();

            Bus.Receive<RabbitResume>("Resume", msg =>
            {
                Resume Resume = new Resume() { Name = msg.Name,};
                ResumesCollection.Push(Resume);
            });
            Thread.Sleep(5000);

            foreach (Resume a in ResumesCollection)
            {
                _context.Add(a);
            }
            _context.SaveChanges();
            return _context.Resumes;
        }


        // GET: api/Resumes/page/{id}
        [HttpGet]
        [Route("page/{page}")]
        public List<Resume> GetResumes([FromRoute] int page = 1)
        {
            var qry = _context.Resumes.OrderBy(p => p.Name);

            PagingList<Resume> ResumeList;
            if (page != 0)
            {
                ResumeList = PagingList.Create(qry, StringsPerPage, page);
            }
            else
            {
                ResumeList = PagingList.Create(qry, _context.Resumes.Count() + 1, 1);
            }

            return ResumeList.ToList();
        }


        // GET: api/Resumes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResume([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Resume = await _context.Resumes.SingleOrDefaultAsync(m => m.ID == id);

            if (Resume == null)
            {
                return NotFound();
            }

            return Ok(Resume);
        }

        // POST: api/Resumes/Find
        [Route("Find")]
        [HttpPost]
        public async Task<IActionResult> FindByName([FromBody] ResumeNameBinding name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Resume = await _context.Resumes.FirstOrDefaultAsync(m => m.Name == name.Name);

            if (Resume == null)
            {
                return NotFound();
            }

            return Ok(Resume);
        }

        // PUT: api/Resumes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResume([FromRoute] int id, [FromBody] Resume Resume)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Resume.ID)
            {
                return BadRequest();
            }

            _context.Entry(Resume).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Accepted(Resume);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResumeExists(id))
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

        // POST: api/Resumes
        [HttpPost]
        public async Task<IActionResult> PostResume([FromBody] Resume Resume)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Resumes.Add(Resume);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResume", new { id = Resume.ID }, Resume);
        }

        // DELETE: api/Resumes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResume([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Resume = await _context.Resumes.SingleOrDefaultAsync(m => m.ID == id);
            if (Resume == null)
            {
                return NotFound();
            }

            _context.Resumes.Remove(Resume);
            await _context.SaveChangesAsync();

            return Ok(Resume);
        }

        private bool ResumeExists(int id)
        {
            return _context.Resumes.Any(e => e.ID == id);
        }

        // GET: api/Resume
        [HttpGet]
        [Route("count")]
        public int GetCountResumes()
        {
            return _context.Resumes.Count();
        }
    }
}