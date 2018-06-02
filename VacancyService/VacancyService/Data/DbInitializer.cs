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
            Resume first = new Resume { Speiality = "Программист", ResumeName = "Конфигурирование и поддержка программ на базе УТ 11 (Платформа 1С:Предприятие 8.3). Доработка функционала типовых конфигураций 1С...Опыт работы программистом 1С 8.2(8.3) от 3 лет", Salary = 200000, Age = 21};
            Resume second = new Resume { Speiality = "Программист 1С", ResumeName = "Программирование и конфигурирование на базе платформы 1С 8. Написание отчетов, обработок, создание печатных форм. ", Salary = 28500, Age = 2 };

            Dictionary<Resume, int> firstVacancy = new Dictionary<Resume, int>();
            firstVacancy.Add(first, 2);

            string firstString = string.Join(";", firstVacancy.Select(x => x.Key.ResumeName + "=" + x.Value).ToArray());


            Dictionary<Resume, int> secondVacancy = new Dictionary<Resume, int>();
            secondVacancy.Add(first, 1);
            secondVacancy.Add(second, 1);

            string secondString = string.Join(";", secondVacancy.Select(x => x.Key.ResumeName + "=" + x.Value).ToArray());

            var Vacancys = new VacancyItems[]
            {

                new VacancyItems("Глухова Ольга Викторовна", "Не готова к командировкам. Не готова к переезду. Управление продажами, маркетинг, мерчендайзинг, Навыки работы с документацией, разработки регламентов, ведения отчетности", 300000, " номер для связи +7925ххххххх") { VacancyName = firstString },
                new VacancyItems("Попов Олег Викторович", "Разработка алгоритмов функционального программного обеспечения бортовой вычислительной машины. - Разработка многофункциональных индикаторов", 23000, "номер для связи +7926ххххххх") { VacancyName = secondString }
            };

            foreach (VacancyItems p in Vacancys)
            {
                context.VacancyItems.Add(p);
            }
            context.SaveChanges();
        }
    }
}
