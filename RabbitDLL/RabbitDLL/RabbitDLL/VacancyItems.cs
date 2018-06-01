using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitDLL
{
    public class VacancyItems
    {
        public int ID { get; set; }
        public string VacancyName { get; set; }
        public string Adress { get; set; }
        public float Salary { get; set; }
        public string UserInfo { get; set; }
        public string VacancyInfo { get; set; }

        public VacancyItems(string adress, string userInfo, float Salary, string VacancyInfo)
        {
            this.Adress = adress;
            this.UserInfo = userInfo;
            this.Salary = Salary;
            this.VacancyInfo = VacancyInfo;
        }

        public VacancyItems() { }


        public static string ItemsStringView(Dictionary<Resume, int> dict)
        {
            string secondString = string.Join(";", dict.Select(x => x.Key.ResumeName + "=" + x.Value).ToArray());
            return secondString;
        }
    }
}
