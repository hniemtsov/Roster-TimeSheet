using RestHomes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestHomes.Domain.Abstract
{
    public interface IFortNightRepository
    {
        IQueryable<FortNight> FortNights { get; }
    }
}
