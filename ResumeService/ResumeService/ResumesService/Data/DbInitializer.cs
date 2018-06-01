using RabbitDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResumesService.Data
{
    public class DbInitializer
    {
        public static void Initialize(ResumeContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Resumes.Any())
            {
                return;   // DB has been seeded
            }

            var Resumes = new Resume[]
            {
            new Resume{ Speiality = "Programmer", ResumeName = "Senior Programmer", Salary = 2000000, Age = 30},
            new Resume{ Speiality = "Business Analythic" , ResumeName = "Leader Business Analythic", Salary = 2500000, Age = 25}
            };
            foreach (Resume p in Resumes)
            {
                context.Resumes.Add(p);
            }
            context.SaveChanges();


            var categories = new Company[]
            {
                new Company{ CompanyName = "Micrisoft"},
                new Company{ CompanyName = "CINIMEX"},
                new Company{ CompanyName = "IBM" }
            };
            foreach (Company c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var Resume_categories = new Resume_Company[]
            {
                new Resume_Company() { CompanyID = 1, ResumeID = 1, Salary = 10000 },
                new Resume_Company() { CompanyID = 2, ResumeID = 1, Salary = 70000 },
                new Resume_Company() { CompanyID = 3, ResumeID = 1, Salary = 70000 },
                new Resume_Company() { CompanyID = 1, ResumeID = 2, Salary = 3000 },
                new Resume_Company() { CompanyID = 2, ResumeID = 2, Salary = 5000 },
                new Resume_Company() { CompanyID = 3, ResumeID = 2, Salary = 30000000 }
            };

            foreach (Resume_Company pc in Resume_categories)
            {
                context.Resume_Categories.Add(pc);
            }
            context.SaveChanges();
        }
    }
}
