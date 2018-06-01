using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitDLL
{
    public class ResumeCortege
    {
        public string ResumeName;
        public Dictionary<string, int> CompanyParameters = new Dictionary<string, int>();
        public float Salary;
        public int Count;

        public ResumeCortege() { }
        public ResumeCortege(string ResumeName, float Salary, int count)
        {
            this.ResumeName = ResumeName;
            this.Salary = Salary;
            this.Count = count;
        }
    }
}
