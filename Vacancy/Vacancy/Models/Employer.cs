using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vacancyService.Models
{
    public class employer
    {
        public int ID { get; set; }
        public string employerName { get; set; }
        public string EmployerAddress { get; set; }

        //public virtual List<vacancy> vacancys { get; set; }
    }
}
