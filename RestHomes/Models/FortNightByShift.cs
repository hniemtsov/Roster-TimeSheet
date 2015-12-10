using RestHomes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestHomes.Models
{
    public class FortNightByShift
    {
        public int shiftID { get; set; }
        public IEnumerable<int?> workerID { get; set; }
    }
}