using ConcerteService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcerteService.Data
{
    public class DbInitializer
    {
        public static void Initialize(ResumeContext context)
        {
            context.Database.EnsureCreated();

            if (context.Resumes.Any())
            {
                return;   // DB has been seeded
            }

            var Resumes = new Resume[]
            {
                new Resume { Name = "Козлова Юлия Алексеевна", City = "Москва", Job="Аэронавигация Московской области». Стажер (производственная практика), инженер, начальник отдела"},
                new Resume { Name = "Глуховской Сергей Викторович", City = "Москва", Job="ООО «ННН-групп» (www.nnn-grup.com), г. Санкт-Петербург.Руководитель отдела продаж"},
                new Resume { Name = "Попова Ольга Викторовна", City = "Омск", Job="Продавец-кассир, ООО «Стиль», магазин мужской одежды, г. Москва"},
                new Resume { Name = "Попов Олег Алексеевич", City = "Воронеж", Job="Медицинский брат по физиотерапии, Медицинский центр «Здоровье», г. Москва"},
  
            };

            foreach (Resume s in Resumes)
            {
                context.Resumes.Add(s);
            }
            context.SaveChanges();
        }
    }
}
