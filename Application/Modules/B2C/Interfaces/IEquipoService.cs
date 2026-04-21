using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fulbacho.Application.Modules.B2C.DTOs;

namespace Fulbacho.Application.Modules.B2C.Services
{
    public interface IEquipoService
    {
        Task<bool> CrearEquipoAsync(CrearEquipoDto dto, int idCapitan);
    }
}
