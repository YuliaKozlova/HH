using RabbitDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacancyService.Data
{
    public class DbInitializer
    {
        public static void Initialize(VacancyContext context)
        {
            context.Database.EnsureCreated();

            if (context.VacancyItems.Any())
            {
                return;   // DB has been seeded
            }
            Resume first = new Resume { Speiality = "Book", ResumeName = "Harry Potter", Salary = 2000, Age = 1 };
            Resume second = new Resume { Speiality = "ComputerItem", ResumeName = "The Witcher 3", Salary = 2500, Age = 2 };

            Dictionary<Resume, int> firstVacancy = new Dictionary<Resume, int>();
            firstVacancy.Add(first, 2);

            string firstString = string.Join(";", firstVacancy.Select(x => x.Key.ResumeName + "=" + x.Value).ToArray());


            Dictionary<Resume, int> secondVacancy = new Dictionary<Resume, int>();
            secondVacancy.Add(first, 1);
            secondVacancy.Add(second, 1);

            string secondString = string.Join(";", secondVacancy.Select(x => x.Key.ResumeName + "=" + x.Value).ToArray());

            var Vacancys = new VacancyItems[]
            {

                new VacancyItems("Geroyev Panfilovcev, 14/53", "Vladimir", 3000, "Во второй половине дня, номер для связи +7925ххххххх") { VacancyName = firstString },
                new VacancyItems("Фрязино, пр. Мира, 10/40", "Наталия", 2300, "Перед доставкой позвонить, номер для связи +7926ххххххх") { VacancyName = secondString }
            };

            foreach (VacancyItems p in Vacancys)
            {
                context.VacancyItems.Add(p);
            }
            context.SaveChanges();
        }
    }
}
