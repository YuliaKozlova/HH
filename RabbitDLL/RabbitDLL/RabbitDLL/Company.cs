using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitDLL
{
    public class Company
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public ICollection<Resume_Company> BoundedWith { get; set; }

        public Company(string CompanyName)
        {
            this.CompanyName = CompanyName;
        }

        public Company() { }
    }
}
