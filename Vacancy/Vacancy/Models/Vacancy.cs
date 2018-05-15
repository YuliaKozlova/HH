using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vacancyService.Models
{
    public class vacancy
    {
        public int ID { get; set; }
        public string vacancyName { get; set; }
        public int salary { get; set; }
        public string about { get; set; }
        public int employerID { get; set; }

        public virtual employer employer { get; set; }
    }
}
