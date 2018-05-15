using vacancyService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vacancyService.Data
{
    public class DbInitializer
    {
        public static void Initialize(vacancyContext context)
        {
            context.Database.EnsureCreated();

            if (context.Employer.Any())
            {
                return;   // DB has been seeded
            }

            var employers= new employer[]
            {
                new employer{employerName = "werwer", EmployerAddress = "sdfsdf"},
                new employer{employerName = "rewrw", EmployerAddress = "dsfsdf"},
                new employer{employerName = "rwerw", EmployerAddress = "dfsdfsdf"}
            };
            foreach (employer c in employers)
            {
                context.Employer.Add(c);
            }
            context.SaveChanges();

            var vacancys = new vacancy[]
            {
                new vacancy{ vacancyName = "rwerwer", employerID = 1, salary = 6000},
                new vacancy{ vacancyName = "erwerwer", employerID = 1, salary = 10000},
                new vacancy{ vacancyName = "ewrwer", employerID = 3, salary = 6000},
            };
            foreach (vacancy a in vacancys)
            {
                context.vacancys.Add(a);
            }
            context.SaveChanges();
        }
    }
}
