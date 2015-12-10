using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestHomes.Domain.Entities
{
    public class Personal
    {
        public string Name { get; set; }
        public string SecondName { get; set;}
    }
    public class FortNight
    {
        public DateTime firstDay { get; set; }
        public double[] Hours { get; set; }
        public Personal Worker { get; set; }
    }
}
