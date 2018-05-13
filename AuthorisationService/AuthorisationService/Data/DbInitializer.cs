using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorisationService.Models;

namespace AuthorisationService.Data
{
    public class DbInitializer
    {
        public static void Initialize(UserContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User{ Login = "July", Password = "asdf"},
                new User{ Login = "July", Password = "asdfg"},
                new User{ Login = "Vova", Password = "asdfasd"},
                new User{ Login = "Georg", Password = "asdasdfadsff"}
            };
            foreach (User s in users)
            {
                context.Users.Add(s);
            }
            context.SaveChanges();
        }
    }
}
