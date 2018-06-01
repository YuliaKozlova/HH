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
    [Route("api/Resumes")]
    public class ResumesController : Controller
    {
        private readonly ResumeContext _context;

        public ResumesController(ResumeContext context)
        {
            _context = context;
        }

        // GET: api/Resumes
        [HttpGet]
        public IEnumerable<Resume> GetResumes()
        {
            return _context.Resumes;
        }


        // GET: api/Resumes/Full
        [Route("Full")]
        [HttpGet]
        public IEnumerable<Resume> GetFullResumesInfo()
        {
            var ResumeInfo = _context.Resumes.Include(c => c.BoundedWith);
            foreach (Resume Resume in ResumeInfo)
            {
                foreach (Resume_Company pc in Resume.BoundedWith)
                {
                    pc.Company = _context.Categories.Find(pc.CompanyID);
                }
            }

            List<Resume> lst = new List<Resume>();
            foreach (Resume prod in ResumeInfo.ToList())
            {
                lst.Add(prod);
            }

            foreach (Resume prod in lst)
            {
                foreach (Resume_Company pc in prod.BoundedWith)
                {
                    pc.Company.BoundedWith = null;
                    pc.Resume = null;
                }
            }
            return lst;
        }


        // GET: api/Resumes/full/cortege
        [Route("full/cortege")]
        [HttpGet]
        public FullView GetFullResumesInfoCortege()
        {
            var ResumeInfo = _context.Resumes.Include(c => c.BoundedWith);
            foreach (Resume Resume in ResumeInfo)
            {
                foreach (Resume_Company pc in Resume.BoundedWith)
                {
                    pc.Company = _context.Categories.Find(pc.CompanyID);
                }
            }

            List<Resume> lst = new List<Resume>();
            foreach (Resume prod in ResumeInfo.ToList())
            {
                lst.Add(prod);
            }

            foreach (Resume prod in lst)
            {
                foreach (Resume_Company pc in prod.BoundedWith)
                {
                    pc.Company.BoundedWith = null;
                    pc.Resume = null;
                }
            }

            //___________________________________________________
            List<ResumeCortege> lstCortege = new List<ResumeCortege>();
            HashSet<string> Categories = new HashSet<string>();
            foreach (Resume Resume in lst)
            {
                ResumeCortege cortege = new ResumeCortege(Resume.ResumeName, Resume.Salary, Resume.Age);
                foreach (Resume_Company bound in Resume.BoundedWith)
                {
                    cortege.CompanyParameters.Add(bound.Company.CompanyName, bound.Salary);
                    Categories.Add(bound.Company.CompanyName);
                }
                lstCortege.Add(cortege);
            }

            FullView objectToView = new FullView() { CompanyCollection = Categories, ResumeCollection = lstCortege };
            return objectToView;
            //___________________________________________________
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

            return NoContent();
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
    }
}