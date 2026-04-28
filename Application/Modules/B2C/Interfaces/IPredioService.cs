using Fulbacho.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fulbacho.Application.Modules.B2C.Interfaces
{
    public interface IPredioService
    {
        Task<IEnumerable<Predio>> GetAllPrediosAsync();
        Task<IEnumerable<Predio>> GetPrediosByZonaAsync(string zona);
    }
}
