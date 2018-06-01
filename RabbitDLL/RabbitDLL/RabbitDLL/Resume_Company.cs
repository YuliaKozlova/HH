using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitDLL
{
    public class Resume_Company
    {
        public int ID { get; set; }
        public int ResumeID { get; set; }
        public int CompanyID { get; set; }
        public int Salary { get; set; }

        public Resume Resume { get; set; }
        public Company Company { get; set; }

        public Resume_Company(Resume Resume, Company Company, int Salary)
        {
            this.Resume = Resume;
            this.Company = Company;
            this.ResumeID = Resume.ID;
            this.CompanyID = Company.ID;
            this.Salary = Salary;
        }

        public Resume_Company() { }
    }
}
