using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitDLL
{
    public class FullView
    {
        public HashSet<string> CompanyCollection = new HashSet<string>();
        public List<ResumeCortege> ResumeCollection = new List<ResumeCortege>();
    }

    public class Resume
    {
        public int ID { get; set; }
        public string Speiality { get; set; }
        public string ResumeName { get; set; }
        public float Salary { get; set; }
        public int Age { get; set; }

        public ICollection<Resume_Company> BoundedWith { get; set; }

        public Resume(string Speiality, string ResumeName, float Salary, int Age)
        {
            this.Speiality = Speiality;
            this.ResumeName = ResumeName;
            this.Salary = Salary;
            this.Age = Age;
        }

        public Resume() { }
    }
}
