using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthorisationService.Data;
using RabbitDLL;
using AuthorisationService.Functions;

namespace AuthorisationService.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.ID)
            {
                return BadRequest();
            }
            var usrDB = _context.Users.Where(s => s.ID == user.ID && s.Login == user.Login).FirstOrDefault<User>();
            usrDB.LastToken = user.LastToken;
            _context.Entry(usrDB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Accepted(usrDB);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User RealUser = new User { Login = user.Login, Password = Hasher.GetHashString(user.Password), Role = "User" };

            _context.Users.Add(RealUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }

        // POST: api/Users/Find
        [Route("Find")]
        [HttpPost]
        public IActionResult CheckUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User RealUser = new User { Login = user.Login, Password = Hasher.GetHashString(user.Password) };

            //CHECK is REALUSER IN DATABASE
            foreach (User u in _context.Users)
            {
                if (u.Login == RealUser.Login && u.Password == RealUser.Password)
                {
                    RealUser.ID = u.ID;
                    RealUser.Role = u.Role;
                    return Ok(RealUser);
                }
            }
            return NoContent();
        }

        // POST: api/Users/FindByLogin
        [Route("FindByLogin")]
        [HttpPost]
        public IActionResult CheckUserWeak([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User RealUser = new User { Login = user.Login, LastToken = user.LastToken };

            //CHECK is REALUSER IN DATABASE
            foreach (User u in _context.Users)
            {
                if (u.Login == RealUser.Login && u.LastToken == RealUser.LastToken)
                {
                    RealUser.ID = u.ID;
                    RealUser.Role = u.Role;
                    RealUser.Password = u.Password;
                    return Ok(RealUser);
                }
            }
            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}