using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcerteService.Models
{
    public class Resume
    {
        public int ID { get; set; }
        public string Job { get; set; }
        public string Name { get; internal set; }
        public string City { get; internal set; }
  
    }
}
